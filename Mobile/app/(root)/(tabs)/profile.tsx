import {Alert, ScrollView, View} from "react-native";
import React, {useState} from "react";
import {useClerk, useUser} from "@clerk/clerk-expo";
import {router} from "expo-router";
import PickUserModal from "@/components/auth/PickUserModal";
import {SafeAreaView} from "react-native-safe-area-context";
import {Button, Text} from "react-native-paper";
import {useDeleteAccountMutation} from "@/behavior/auth/authApi";
import {useToast} from "react-native-paper-toast";
import {useAppDispatch} from "@/behavior/hooks";
import {clearTokens, setSessionId} from "@/behavior/auth/authSlice";

const Profile = () => {
  const toast = useToast();
  const dispatch = useAppDispatch();
  const {client, setActive, signOut, session} = useClerk();
  const {user} = useUser();
  const [showProfilesModal, setShowProfilesModal] = useState(false);
  const [deleteAccount] = useDeleteAccountMutation();

  const toggleShowProfilesModal = () => {
    setShowProfilesModal(!showProfilesModal);
  };

  async function handleSignOut() {
    await signOut({sessionId: session?.id});
    dispatch(clearTokens());
    dispatch(setSessionId(''));
    if (client.activeSessions[0]) {
      await setActive({session: client.activeSessions[0]});
      router.replace("/(root)/(tabs)/home");
    } else {
      router.replace("/(auth)/welcome");
    }
  }

  async function handleAccountDeletion() {

    const res = await deleteAccount(null)
      .unwrap()
      .catch(err => toast.show({type: "error", message: err.data}));
    console.log('delete res', res);
    if (!res)
      return;
    await signOut({sessionId: session?.id});
    dispatch(setSessionId(''));
    if (client.activeSessions[0]) {
      await setActive({session: client.activeSessions[0]});
      dispatch(clearTokens());
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
          <Text variant='bodyMedium'>Current user: {user?.emailAddresses[0].emailAddress}</Text>
          <Button mode='contained'
                  onPress={() => toggleShowProfilesModal()}
          >Change profile</Button>
          <Button mode='contained' onPress={handleSignOut}>Sign out</Button>
          <Button mode='contained' onPress={handleAccountDeletion}>Delete my account</Button>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

export default Profile;
