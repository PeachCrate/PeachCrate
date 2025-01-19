import {SafeAreaView} from "react-native-safe-area-context";
import {TouchableOpacity, View} from "react-native";
import {router, useRouter} from "expo-router";
import React, {useEffect, useMemo, useState} from "react";
import {useClerk} from "@clerk/clerk-expo";
import {Button, Text} from "react-native-paper";
import {usePingQuery} from "@/behavior/auth/authApi";
import PickUserModal from "@/components/auth/PickUserModal";
import {useAppSelector} from "@/behavior/hooks";

const Welcome = () => {
  const {client} = useClerk();

  const showAccountsButton = useMemo(() => {
    return client.activeSessions.length > 0;
  }, [client.activeSessions]);
  const {data: isConnected} = usePingQuery(null);

  const router = useRouter();
  const sessionId = useAppSelector(state => state.auth.sessionId);
  useEffect(() => {
    if (sessionId)
      router.replace('/(root)/(tabs)/home')
  }, [sessionId]);

  const [showProfilesModal, setShowProfilesModal] = useState(false);
  return (
    <SafeAreaView className="flex h-full items-center justify-between">
      {showAccountsButton && (
        <Button onPress={() => {
          setShowProfilesModal(true);
        }} className="w-full flex justify-start items-start p-5">Pick available account</Button>
      )}

      <View>
        <Text>{isConnected ? "Connection with the server established" : "No connection to the server"}</Text>
      </View>

      <View className="pb-96">
        <Text variant="bodyLarge">Our beautiful welcome page</Text>
      </View>
      <View className="flex flex-row p-5 w-full">
        {isConnected &&
            <>
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
            </>
        }
        <Button
          mode="contained"
          icon="account-cancel"
          className="flex flex-col flex-1"
          onPress={() => {
            router.replace("/(root)/(tabs)/home");
          }}
        >
          Stay guest
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
