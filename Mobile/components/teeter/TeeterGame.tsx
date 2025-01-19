import React, {useEffect, useState} from 'react';
import {View, StyleSheet, Dimensions, Alert, Animated, ViewBase} from 'react-native';
import {DeviceMotion} from 'expo-sensors';
import {useAppDispatch, useAppSelector} from "@/behavior/hooks";
import {setRunningGame} from "@/behavior/game/gameSlice";
import {Button, useTheme} from "react-native-paper";
import {useToast} from "react-native-paper-toast";
import {SafeAreaView} from "react-native-safe-area-context";
import {Is} from "@sinclair/typebox/value/is";

const {width: dimensionsWidth, height: dimensionsHeight} = Dimensions.get('window');
const width = dimensionsWidth;
const height = dimensionsHeight - 300;
const BALL_SIZE = 30;
const HOLE_SIZE = 50;
const WALL_THICKNESS = 20;
const BALL_INIT_X = 40;
const BALL_INIT_Y = 40;

type Point = {
  x: number,
  y: number,
}

const TeeterGame = () => {
  const toast = useToast();
  const theme = useTheme();
  const running = useAppSelector(state => state.game.running)
  const [isRunning, setIsRunning] = useState(running);
  const dispatch = useAppDispatch();
  const [ballPosition, setBallPosition] = useState({
    x: BALL_INIT_X,
    y: BALL_INIT_Y,
  });
  const ballAnimated = new Animated.ValueXY(ballPosition);

  const holes = [
    {x: 25, y: 70},
    {x: 90, y: 70},
    {x: 140, y: 70},
    {x: 190, y: 70},
    {x: 260, y: 70},
    {x: 310, y: 70},
    {x: 300, y: 440}
  ];

  const walls = [
    {x: 0, y: 0, width: width, height: WALL_THICKNESS}, // Top wall
    {x: 0, y: height - WALL_THICKNESS, width: width, height: WALL_THICKNESS}, // Bottom wall
    {x: 0, y: 0, width: WALL_THICKNESS, height: height}, // Left wall
    {x: width - WALL_THICKNESS, y: 0, width: WALL_THICKNESS, height: height}, // Right wall
  ];
  const isBallInHole = (ballPosition: Point, hole: Point): {isBallInHole: boolean, hole: Point} => {
    // Calculate center points of both the ball and the hole
    const ballCenter = {x: ballPosition.x + BALL_SIZE / 2, y: ballPosition.y + BALL_SIZE / 2};
    const holeCenter = {x: hole.x + HOLE_SIZE / 2, y: hole.y + HOLE_SIZE / 2};
    // Calculate the distance between the centers
    const distance = Math.sqrt(
      Math.pow(ballCenter.x - holeCenter.x, 2) + Math.pow(ballCenter.y - holeCenter.y, 2)
    );
    // Check if the ball's radius plus some threshold is smaller than the distance to the hole's center
    return { isBallInHole: distance < (HOLE_SIZE / 2 - BALL_SIZE / 2) + 10, hole: hole };
  };

  useEffect(() => {
    console.log('isRuuning', isRunning)
      dispatch(setRunningGame(isRunning))
  }, [isRunning]);

  useEffect(() => {
    setIsRunning(running);
  }, [running]);
  
  useEffect(() => {
    const subscription = DeviceMotion.addListener((motionData) => {
      if (!running) return;

      const {accelerationIncludingGravity} = motionData;
      if (accelerationIncludingGravity) {
        const forceX = accelerationIncludingGravity.x * 2;
        const forceY = accelerationIncludingGravity.y * -2;

        setBallPosition((prev) => {
          let newX = prev.x + forceX;
          let newY = prev.y + forceY;

          newX = Math.max(WALL_THICKNESS, Math.min(width - WALL_THICKNESS - BALL_SIZE, newX));
          newY = Math.max(WALL_THICKNESS, Math.min(height - WALL_THICKNESS - BALL_SIZE, newY));

          for (let hole of holes) {
            const isInHole = isBallInHole({x: newX, y: newY}, hole);
            const isWin = holes[holes.length-1] === isInHole.hole;
            if (isInHole.isBallInHole && !isWin) {
              restartGame();
              return prev; // Stop updating position
            }
            else if (isInHole.isBallInHole && isWin) {
              restartGame();
              setIsRunning(false);
              Alert.alert('You won!');
              return prev; // Stop updating position
            }
          }
          ballAnimated.setValue({x: newX, y: newY});
          return {x: newX, y: newY};
        });
      }
    });

    DeviceMotion.setUpdateInterval(8); // Update at ~60Hz

    return () => {
      subscription.remove();
      DeviceMotion.removeAllListeners();
    };
  }, [running]);

  const restartGame = () => {
    dispatch(setRunningGame(true));
    setBallPosition({x: BALL_INIT_X, y: BALL_INIT_Y});
    ballAnimated.setValue({x: BALL_INIT_X, y: BALL_INIT_Y});
  };

  return (
    <>
      <SafeAreaView>
        <Button onPress={() => dispatch(setRunningGame(true))}>Resume game</Button>
        <Button onPress={() => dispatch(setRunningGame(false))}>Pause game</Button>
        <Button onPress={restartGame}>Restart game</Button>
        <View style={styles.container}>
          <View>
            
            {holes.map((hole, index) => {
              let color = theme.dark ? 'rgb(52, 52, 52)' : 'black';
              color = holes.length -1 === index ? 'blue' : color
              return (
              <View
                key={index}
                style={[
                  styles.hole,
                  {
                    left: hole.x,
                    top: hole.y,
                    backgroundColor: color
                  },
                ]}
              />
            )})}

            {walls.map((wall, index) => (
              <View
                key={index}
                style={[
                  styles.wall,
                  {
                    left: wall.x,
                    top: wall.y,
                    width: wall.width,
                    height: wall.height,
                    backgroundColor: theme.dark ? 'rgb(100, 100, 100)' : 'gray'
                  },
                ]}
              />
            ))}

            <Animated.View
              style={[
                styles.ball,
                {
                  transform: [
                    {translateX: ballAnimated.x},
                    {translateY: ballAnimated.y},
                  ],
                },
              ]}
            />
          </View>
        </View>
      </SafeAreaView>
    </>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f0f0f0',
  },
  hole: {
    position: 'absolute',
    width: HOLE_SIZE,
    height: HOLE_SIZE,
    borderRadius: HOLE_SIZE / 2,
    backgroundColor: 'black',
  },
  wall: {
    position: 'absolute',
    backgroundColor: 'gray',
  },
  ball: {
    position: 'absolute',
    width: BALL_SIZE,
    height: BALL_SIZE,
    borderRadius: BALL_SIZE / 2,
    backgroundColor: 'red',
  },
});

export default TeeterGame;
