import {SafeAreaView} from "react-native-safe-area-context";
import {Text} from "react-native";
import {Redirect} from "expo-router";

const Home = () => {
  return <Redirect href="/(auth)/welcome" />;
};

export default Home;