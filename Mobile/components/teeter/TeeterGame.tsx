import React, {useEffect, useState} from 'react';
import {View, StyleSheet, Dimensions, Alert, Animated} from 'react-native';
import {DeviceMotion} from 'expo-sensors';
import {useAppDispatch, useAppSelector} from "@/behavior/hooks";
import {setRunningGame} from "@/behavior/game/gameSlice";
import {Button} from "react-native-paper";
import {useToast} from "react-native-paper-toast";
import {SafeAreaView} from "react-native-safe-area-context";

const {width: dimensionsWidth, height: dimensionsHeight} = Dimensions.get('window');
const width = dimensionsWidth;
const height = dimensionsHeight-220;
const BALL_SIZE = 30;
const HOLE_SIZE = 50;
const WALL_THICKNESS = 20;

const isBallInHole = (ballPosition, hole) => {

  // Calculate center points of both the ball and the hole
  const ballCenter = {x: ballPosition.x + BALL_SIZE / 2, y: ballPosition.y + BALL_SIZE / 2};
  const holeCenter = {x: hole.x + HOLE_SIZE / 2, y: hole.y + HOLE_SIZE / 2};
  // Calculate the distance between the centers
  const distance = Math.sqrt(
    Math.pow(ballCenter.x - holeCenter.x, 2) + Math.pow(ballCenter.y - holeCenter.y, 2)
  );
  // Check if the ball's radius plus some threshold is smaller than the distance to the hole's center
  return distance < (HOLE_SIZE / 2 - BALL_SIZE / 2) + 10;
};
const TeeterGame = () => {
  const toast = useToast();
  const running = useAppSelector(state => state.game.running)
  const dispatch = useAppDispatch();
  // const [ballPosition, setBallPosition] = useState({
  //   x: width / 2 - BALL_SIZE / 2,
  //   y: height / 2 - BALL_SIZE / 2,
  // });
  const [ballPosition, setBallPosition] = useState({
    x: BALL_SIZE / 2,
    y: BALL_SIZE / 2,
  });
  const ballAnimated = new Animated.ValueXY(ballPosition);

  const holes = [
    {x: 100, y: 200},
    {x: 250, y: 400},
    {x: 300, y: 300},
  ];

  const walls = [
    {x: 0, y: 0, width: width, height: WALL_THICKNESS}, // Top wall
    {x: 0, y: height - WALL_THICKNESS, width: width, height: WALL_THICKNESS}, // Bottom wall
    {x: 0, y: 0, width: WALL_THICKNESS, height: height}, // Left wall
    {x: width - WALL_THICKNESS, y: 0, width: WALL_THICKNESS, height: height}, // Right wall

    // Additional walls inside the game area
    {x: 100, y: 100, width: 100, height: WALL_THICKNESS},  // Horizontal wall 1
    {x: 200, y: 200, width: WALL_THICKNESS, height: 100},  // Vertical wall 1
    {x: 150, y: 300, width: 100, height: WALL_THICKNESS},  // Horizontal wall 2
  ];

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

          // Prevent ball from going through walls
          newX = Math.max(WALL_THICKNESS, Math.min(width - WALL_THICKNESS - BALL_SIZE, newX));
          newY = Math.max(WALL_THICKNESS, Math.min(height - WALL_THICKNESS - BALL_SIZE, newY));

          // Check if ball is in any hole
          for (let hole of holes) {
            if (isBallInHole({x: newX, y: newY}, hole)) {
              restartGame();
              return prev; // Stop updating position
            }
          }
          ballAnimated.setValue({x: newX, y: newY});
          return {x: newX, y: newY};
        });
      }
    });

    DeviceMotion.setUpdateInterval(16); // Update at ~60Hz

    return () => {
      subscription.remove();
      DeviceMotion.removeAllListeners();
    };
  }, [running]);

  const restartGame = () => {
    dispatch(setRunningGame(true));
    setBallPosition({x: BALL_SIZE / 2, y: BALL_SIZE / 2});
    ballAnimated.setValue({x: BALL_SIZE / 2, y: BALL_SIZE / 2});
  };

  return (
    <>
      <SafeAreaView>
        <Button onPress={() => dispatch(setRunningGame(true))}>Resume game</Button>
        <Button onPress={() => dispatch(setRunningGame(false))}>Pause game</Button>
        <Button onPress={restartGame}>Restart game</Button>
        <View style={styles.container}>
          {holes.map((hole, index) => (
            <View
              key={index}
              style={[
                styles.hole,
                {
                  left: hole.x,
                  top: hole.y,
                },
              ]}
            />
          ))}

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
