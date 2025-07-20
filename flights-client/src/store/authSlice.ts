import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';

interface AuthState {
    user: string | null;
    token: string | null;
    loading: boolean;
    error: string | null;
}

const initialState: AuthState = {
    user: localStorage.getItem('user'),
    token: localStorage.getItem('token'),
    loading: false,
    error: null,
};

// Authentication slice for user state
const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        loginStart(state) {
            state.loading = true;
            state.error = null;
        },
        loginSuccess(state, action: PayloadAction<{ user: string; token: string }>) {
            state.loading = false;
            state.user = action.payload.user;
            state.token = action.payload.token;
            state.error = null;
            localStorage.setItem('user', action.payload.user);
            localStorage.setItem('token', action.payload.token);
        },
        loginFailure(state, action: PayloadAction<string>) {
            state.loading = false;
            state.error = action.payload;
        },
        logout(state) {
            state.user = null;
            state.token = null;
            state.error = null;
            localStorage.removeItem('user');
            localStorage.removeItem('token');
        },
        registerStart(state) {
            state.loading = true;
            state.error = null;
        },
        registerSuccess(state, action: PayloadAction<{ user: string; token: string }>) {
            state.loading = false;
            state.user = action.payload.user;
            state.token = action.payload.token;
            state.error = null;
            localStorage.setItem('user', action.payload.user);
            localStorage.setItem('token', action.payload.token);
        },
        registerFailure(state, action: PayloadAction<string>) {
            state.loading = false;
            state.error = action.payload;
        },
    },
});

export const {
    loginStart,
    loginSuccess,
    loginFailure,
    logout,
    registerStart,
    registerSuccess,
    registerFailure,
} = authSlice.actions;

export default authSlice.reducer; 