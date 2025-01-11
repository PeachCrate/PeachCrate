import { SafeAreaView } from "react-native-safe-area-context";
import { TouchableOpacity, View } from "react-native";
import { router } from "expo-router";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faSpider } from "@fortawesome/free-solid-svg-icons/faSpider";
import { faMugSaucer } from "@fortawesome/free-solid-svg-icons/faMugSaucer";
import CustomButton from "@/components/CustomButton";
import React, { useMemo, useState } from "react";
import { useClerk } from "@clerk/clerk-expo";
import PickUserModal from "@/components/PickUserModal";
import { Button, Text } from "react-native-paper";

const Welcome = () => {
  const { client } = useClerk();
  const showAccountsButton = useMemo(() => {
    return client.activeSessions.length > 0;
  }, [client.activeSessions]);

  const [showProfilesModal, setShowProfilesModal] = useState(false);
  return (
    <SafeAreaView className="flex h-full items-center justify-between">
      {showAccountsButton && (
        <TouchableOpacity
          onPress={() => {
            setShowProfilesModal(true);
          }}
          className="w-full flex justify-start items-start p-5"
        >
          <Text className="text-black text-md font-JakartaBold" >
            Pick available account
          </Text>
        </TouchableOpacity>
      )}
      <View className="flex pb-96">
        <Text variant="bodyLarge">Our beautiful welcome page</Text>
      </View>
      <View className="flex flex-row p-5 w-full">
        <Button
          mode="contained"
          icon="spider"
          className="flex flex-col flex-1"
          onPress={() => {
            router.replace("/(auth)/register");
          }}
        >
          Register
        </Button>
        <Button
          mode="contained"
          icon="coffee"
          className="flex flex-col flex-1"
          onPress={() => {
            router.replace("/(auth)/login");
          }}
        >
          Login
        </Button>
      </View>
      <PickUserModal
        showModal={showProfilesModal}
        setShowModal={setShowProfilesModal}
        checkSingleUser={false}
      />
    </SafeAreaView>
  );
};

export default Welcome;
