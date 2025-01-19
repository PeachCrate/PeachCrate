import {ScrollView } from "react-native";
import {router} from "expo-router";
import React from "react";
import {useClerk, useUser} from "@clerk/clerk-expo";
import {useToast} from "react-native-paper-toast";
import {Button, Dialog, Portal, RadioButton, Text} from "react-native-paper";
import {useAppDispatch} from "@/behavior/hooks";
import {setSessionId} from "@/behavior/auth/authSlice";

type PickUserModalProps = {
  showModal: boolean;
  setShowModal: React.Dispatch<React.SetStateAction<boolean>>;
  checkSingleUser?: boolean;
}
const PickUserModal = ({showModal, setShowModal, checkSingleUser = true}: PickUserModalProps) => {
  const toast = useToast();
  const {client, setActive} = useClerk();
  const {user} = useUser();
  const dispatch = useAppDispatch();

  const hideModal = () => {
    setShowModal(false);
  }

  function handleProfileSwitch(userId: string) {
    if (userId === user?.id && client.activeSessions.length > 2) return;
    const session = client.sessions.find((s) => s?.user?.id === userId);
    setActive({session: session?.id}).catch((err: any) => console.error(err));
    dispatch(setSessionId(session?.id!));
    hideModal();
    router.replace("/(root)/(tabs)/home");
  }

  function getCurrentUserId() {
    if (!checkSingleUser) return null;
    return user?.id;
  }

  return (
    <Portal>
      <Dialog visible={showModal} onDismiss={hideModal}>
        <Dialog.Title>Choose one of profiles</Dialog.Title>
        <Dialog.Content>
          <ScrollView>
            <RadioButton.Group
              onValueChange={handleProfileSwitch}
              value={!getCurrentUserId() ? '' : getCurrentUserId()!}
            >
              {client.sessions.map((s) => {
                return (
                  <RadioButton.Item
                    key={s.id}
                    value={s.user?.id!}
                    label={s.user?.emailAddresses[0].emailAddress!}
                  />
                );
              })}
            </RadioButton.Group>
            <Button
              mode='contained'
              onPress={() => {
                router.push("/(auth)/welcome");
                dispatch(setSessionId(''));
                hideModal();
              }}
            >Add new account</Button>
          </ScrollView>
        </Dialog.Content>
        <Dialog.Actions>
          <Button onPress={hideModal}>Cancel</Button>
        </Dialog.Actions>
      </Dialog>
    </Portal>
  );
};

export default PickUserModal;
