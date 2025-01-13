import {ScrollView, Text, View} from "react-native";
import React, {useState} from "react";
import CustomButton from "@/components/CustomButton";
import {useClerk, useUser} from "@clerk/clerk-expo";
import {router} from "expo-router";
import PickUserModal from "@/components/PickUserModal";
import {SafeAreaView} from "react-native-safe-area-context";

const Profile = () => {
  const {client, setActive, signOut, session} = useClerk();
  const {user} = useUser();
  const [showProfilesModal, setShowProfilesModal] = useState(false);

  const toggleShowProfilesModal = () => {
    setShowProfilesModal(!showProfilesModal);
  };

  async function handleSignOut() {
    await signOut({sessionId: session?.id});
    if (client.activeSessions[0]) {
      setActive({session: client.activeSessions[0]});
      router.replace("/(root)/(tabs)/home");
    } else {
      router.replace("/(auth)/welcome");
    }
  }

  return (
    <SafeAreaView>
      <ScrollView>
        <PickUserModal
          showModal={showProfilesModal}
          setShowModal={setShowProfilesModal}
        />
        <View>
          <Text>Current user: {user?.emailAddresses[0].emailAddress}</Text>
          <CustomButton
            title={"Change profile"}
            onPress={() => toggleShowProfilesModal()}
          />
          <CustomButton title={"Sign out"} onPress={handleSignOut}/>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

export default Profile;
