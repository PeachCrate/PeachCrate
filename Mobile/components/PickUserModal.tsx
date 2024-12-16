import { ReactNativeModal } from "react-native-modal";
import { Alert, ScrollView, Text, TouchableOpacity, View } from "react-native";
import RadioButtonGroup, { RadioButtonItem } from "expo-radio-button";
import CustomButton from "@/components/CustomButton";
import { router } from "expo-router";
import React from "react";
import { useClerk, useUser } from "@clerk/clerk-expo";
import { FontAwesomeIcon } from "@fortawesome/react-native-fontawesome";
import { faXmark } from "@fortawesome/free-solid-svg-icons";

const PickUserModal = ({
  showModal,
  setShowModal,
  checkSingleUser = true,
}: {
  showModal: boolean;
  setShowModal: React.Dispatch<React.SetStateAction<boolean>>;
  checkSingleUser?: boolean;
}) => {
  const { client, setActive } = useClerk();
  const { user } = useUser();

  function handleProfileSwitch(userId: string) {
    if (userId === user?.id && client.activeSessions.length > 2) return;
    const session = client.sessions.find((s) => s?.user?.id === userId);
    setActive({ session: session?.id }).catch((err: any) => Alert.alert(err));
    router.replace("/(root)/(tabs)/home");
  }

  function getCurrentUserId() {
    if (!checkSingleUser) return null;
    return user?.id;
  }

  return (
    <ReactNativeModal isVisible={showModal}>
      <View className="bg-white px-7 py-9 rounded-2xl min-h-[300px] flex  justify-end">
        <TouchableOpacity
          onPress={() => {
            setShowModal(false);
          }}
          className="w-full flex justify-start items-end p-2 mb-4"
        >
          <FontAwesomeIcon icon={faXmark} />
        </TouchableOpacity>
        <Text className="text-3xl font-JakartaBold text-center justify-between">
          Choose one of profiles
        </Text>
        <ScrollView>
          <RadioButtonGroup
            onSelected={handleProfileSwitch}
            className="justify-between"
            selected={getCurrentUserId()}
          >
            {client.sessions.map((s) => {
              return (
                <RadioButtonItem
                  key={s.id}
                  value={s.user?.id}
                  label={<Text>{s.user?.emailAddresses[0].emailAddress}</Text>}
                />
              );
            })}
          </RadioButtonGroup>
          <CustomButton
            title={"Add new account"}
            onPress={() => router.push("/(auth)/welcome")}
          />
        </ScrollView>
      </View>
    </ReactNativeModal>
  );
};

export default PickUserModal;
