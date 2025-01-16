import {IconDefinition} from "@fortawesome/fontawesome-svg-core";
import {View} from "react-native";
import {FontAwesomeIcon} from "@fortawesome/react-native-fontawesome";
import React, {memo} from "react";
import {Icon, useTheme} from "react-native-paper";

type TabIconProps = {
  source: string;
  focused: boolean;
}

const TabIcon = ({source, focused}: TabIconProps) => {
  const theme = useTheme();
  return (<>
    <View>
      <Icon source={`${source}${focused ? '-outline': ''}`} size={30} color={theme.colors.primary} />
    </View>
  </>);
}


export default memo(TabIcon);