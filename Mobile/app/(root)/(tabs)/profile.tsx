import { Alert, ScrollView, Text, View } from "react-native";
import React, { useState } from "react";
import CustomButton from "@/components/CustomButton";
import { ReactNativeModal } from "react-native-modal";
import { useClerk, useUser } from "@clerk/clerk-expo";
import RadioButtonGroup, { RadioButtonItem } from "expo-radio-button";
import { router } from "expo-router";

const Profile = () => {
  const { client, setActive, signOut, session } = useClerk();
  const { user } = useUser();
  const [showProfilesModal, setShowProfilesModal] = useState(false);

  const toggleShowProfilesModal = () => {
    setShowProfilesModal(!showProfilesModal);
  };

  function handleProfileSwitch(userId: string) {
    if (userId === user?.id) return;
    const session = client.sessions.find((s) => s?.user?.id === userId);
    setActive({ session: session?.id }).catch((err: any) => Alert.alert(err));
    router.replace("/(root)/(tabs)/home");
  }

  async function handleSignOut() {
    await signOut({ sessionId: session?.id });
    if (client.activeSessions[0]) {
      setActive({ session: client.activeSessions[0] });
      router.replace("/(root)/(tabs)/home");
    } else {
      router.replace("/(auth)/welcome");
    }
  }

  return (
    <ScrollView>
      <ReactNativeModal isVisible={showProfilesModal}>
        <View className="bg-white px-7 py-9 rounded-2xl min-h-[300px] flex  justify-end">
          <Text className="text-3xl font-JakartaBold text-center justify-between">
            Choose one of profiles
          </Text>
          <ScrollView>
            <RadioButtonGroup
              onSelected={handleProfileSwitch}
              className="justify-between"
              selected={user?.id}
            >
              {client.sessions.map((s) => {
                return (
                  <RadioButtonItem
                    key={s.id}
                    value={s.user?.id}
                    label={
                      <Text>{s.user?.emailAddresses[0].emailAddress}</Text>
                    }
                  />
                );
              })}
            </RadioButtonGroup>
            <CustomButton
              title={"Add new account"}
              onPress={() => router.replace("/(auth)/welcome")}
            />
          </ScrollView>
        </View>
      </ReactNativeModal>

      <View>
        <Text>Current user: {user?.emailAddresses[0].emailAddress}</Text>
        <CustomButton
          title={"Change profile"}
          onPress={() => toggleShowProfilesModal()}
        />
        <CustomButton title={"Sign out"} onPress={handleSignOut} />
      </View>
    </ScrollView>
  );
};

export default Profile;
