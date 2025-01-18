import {createSlice} from "@reduxjs/toolkit";
import type {PayloadAction} from "@reduxjs/toolkit";
import {Tokens} from "@/behavior/auth/types";
import * as SecureStore from 'expo-secure-store';

export interface GameState {
  running: boolean;
}

const initialState: GameState = {
  running: false,
};

export const gameSlice = createSlice({
  name: "game",
  initialState,
  reducers: {
    setRunningGame: (state, action: PayloadAction<boolean>) => {
      state.running = action.payload;
    },
  },
});

// Action creators are generated for each case reducer function
export const {setRunningGame} =
  gameSlice.actions;

export default gameSlice.reducer;
