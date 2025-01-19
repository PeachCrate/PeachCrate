// Import necessary modules and components
import React, {useEffect, useRef, useState} from 'react';
import {
  Alert,
  Image,
  Linking,
  StyleSheet,
  TouchableOpacity,
  View,
} from 'react-native';
import {SafeAreaView} from 'react-native-safe-area-context';
import {
  Camera,
  useCameraDevice,
  useCameraPermission,
  useCodeScanner,
} from 'react-native-vision-camera';
import {Button, Dialog, Icon, Portal, Text} from "react-native-paper";
import {useAppDispatch} from "@/behavior/hooks";
import {barcodeApi} from "@/behavior/barcode/barcodeApi";
import {Product, Root} from '@/behavior/barcode/types';
import {useIsFocused} from "@react-navigation/native";
import {useRouter} from "expo-router";

const ScannerScreen = () => {
  const dispatch = useAppDispatch();
  const router = useRouter();
  const [productInfo, setProductInfo] = useState<Root>();
  const [showProductInfo, setShowProductInfo] = useState(false);
  const isFocused = useIsFocused();
  const [torchOn, setTorchOn] = useState(false);
  const [enableOnCodeScanned, setEnableOnCodeScanned] = useState(true);

  const getGeneralTitle = (productInfo: Root) => {
    if (productInfo.product.generic_name)
      return productInfo.product.generic_name;
    if (productInfo.product.generic_name_en)
      return productInfo.product.generic_name_en;
    if (productInfo.product.generic_name_it)
      return productInfo.product.generic_name_it;
    if (productInfo.product.generic_name_es)
      return productInfo.product.generic_name_es;
    if (productInfo.product.generic_name_fr)
      return productInfo.product.generic_name_fr;
    return '';
  }

  const getProductName = (productInfo: Root) => {
    if (productInfo.product.product_name)
      return productInfo.product.product_name;
    if (productInfo.product.product_name_en)
      return productInfo.product.product_name_en;
    if (productInfo.product.product_name_it)
      return productInfo.product.product_name_it;
    if (productInfo.product.product_name_es)
      return productInfo.product.product_name_es;
    if (productInfo.product.product_name_fr)
      return productInfo.product.product_name_fr;
    return '';
  }

  const {
    hasPermission: cameraHasPermission,
    requestPermission: requestCameraPermission,
  } = useCameraPermission();

  const device = useCameraDevice('back');

  useEffect(() => {
    const checkPermission = async () => {
      if ((cameraHasPermission === null || !cameraHasPermission) && isFocused) {
        console.log('perm', cameraHasPermission);
        const granted = await requestCameraPermission();
        if (!granted) {
          Alert.alert('Permision not granted!',
            'Camera permission is required to use the camera. Please grant permission in your device settings.', [
              {
                text: 'Go to settings',
                onPress: async () => {
                  await Linking.openSettings();
                  const granted = await requestCameraPermission();
                  if (!granted)
                    router.back();
                }
              },
              {
                text: 'Dismiss',
                onPress: () => router.back()
              }
            ]
          );

        }
      }
    };

    checkPermission();
  }, [cameraHasPermission, isFocused]);

  // Use the code scanner hook to configure barcode scanning
  const codeScanner = useCodeScanner({
    codeTypes: ['ean-13', 'code-128'],
    onCodeScanned: async (codes) => {
      // Check if code scanning is enabled
      if (enableOnCodeScanned) {
        let value = codes[0]?.value;

        console.log(codes[0]);

        if (!value)
          return;

        // Disable code scanning to prevent rapid scans
        setEnableOnCodeScanned(false);

        const info = await dispatch(
          barcodeApi.endpoints.getInfoAboutProduct.initiate(value)
        ).unwrap();

        setProductInfo(info);
        setShowProductInfo(true);
      }
    },
  });


  const RoundButtonWithImage = () => {
    return (
      <TouchableOpacity
        onPress={() => setTorchOn((prev) => !prev)}
        style={styles.buttonContainer}>
        <View style={styles.button}>
          <Icon source={torchOn ? 'flashlight' : 'flashlight-off'} size={20}/>
        </View>
      </TouchableOpacity>
    );
  };

  const Instruction = () => {
    return (<>
        <Text style={{
          alignItems: 'center',
          position: 'absolute',
          zIndex: 1,
          top: 40,
        }} variant='headlineMedium'>Tap on the screen to scan</Text>
      </>
    );
  };

  // Render content based on camera device availability
  if (device == null)
    return (
      <View
        style={{flex: 1, justifyContent: 'center', alignItems: 'center'}}
      >
        <Text style={{margin: 10}}>Camera Not Found</Text>
      </View>
    );

  const dismiss = () => {
    setShowProductInfo(false);
    setProductInfo(undefined);
  }

  // Return the main component structure
  return (
    <SafeAreaView style={{flex: 1}}>
      {cameraHasPermission && <>
          <Instruction/>
          <RoundButtonWithImage/>
          <Camera
              codeScanner={codeScanner}
              style={StyleSheet.absoluteFill}
              device={device}
              isActive={true}
              torch={torchOn ? 'on' : 'off'}
              onTouchEnd={() => setEnableOnCodeScanned(true)}
          />
      </>}
      <Portal>
        <Dialog visible={showProductInfo} onDismiss={dismiss}>
          <Dialog.Title>Product info</Dialog.Title>
          <Dialog.Content>
            {(productInfo && productInfo.status === 1) ?
              <>
                <Text>Id: {productInfo.product.id}</Text>
                {getProductName(productInfo) && <Text>Product name: {getProductName(productInfo)}</Text>}
                {getGeneralTitle(productInfo) && <Text>General title: {getGeneralTitle(productInfo)}</Text>}
                {productInfo.product.food_groups && <Text>Food group: {productInfo.product.food_groups}</Text>}
                {productInfo.product.brands && <Text>Brand: {productInfo.product.brands}</Text>}
              </>
              : <>
                <Text>Error during searching info</Text>
              </>
            }
          </Dialog.Content>
          <Dialog.Actions>
            <Button onPress={dismiss}>Close</Button>
          </Dialog.Actions>
        </Dialog>
      </Portal>
    </SafeAreaView>
  );
}

export default ScannerScreen;

// Styles for the component
const styles = StyleSheet.create({
  buttonContainer: {
    alignItems: 'center',
    position: 'absolute',
    zIndex: 1,
    right: 20,
    top: 30,
  },
  button: {
    backgroundColor: '#FFF', // Button background color
    borderRadius: 50, // Make it round (half of the width and height)
    width: 50,
    height: 50,
    justifyContent: 'center',
    alignItems: 'center',
  },
  buttonImage: {
    width: 25, // Adjust the width and height of the image as needed
    height: 25,
  },
});