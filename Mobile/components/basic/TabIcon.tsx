import {IconDefinition} from "@fortawesome/fontawesome-svg-core";
import {View} from "react-native";
import {FontAwesomeIcon} from "@fortawesome/react-native-fontawesome";
import React, {memo} from "react";
import {Icon, useTheme} from "react-native-paper";

type TabIconProps = {
  source: string;
  focused: boolean;
  focusedSource?: string;
}

const TabIcon = ({source, focused, focusedSource}: TabIconProps) => {
  const theme = useTheme();
  const getSource = () => {
    if (!focused){
      return source;
    }
    if (focusedSource)
      return focusedSource;
    return `${source}-outline`
  }
  return (<>
    <View>
      <Icon source={getSource()} size={30} color={theme.colors.primary} />
    </View>
  </>);
}


export default memo(TabIcon);