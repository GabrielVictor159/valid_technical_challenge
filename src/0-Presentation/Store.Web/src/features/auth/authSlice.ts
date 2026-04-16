import { createSlice, type PayloadAction } from '@reduxjs/toolkit'

interface AuthState {
    user: {
        username: string | undefined;
        given_name: string | undefined;
        email: string | undefined;
        roles: string[];
    } | null;
    isAuthenticated: boolean;
    token: string | null;
}

const initialState: AuthState = {
    user: null,
    isAuthenticated: false,
    token: null,
}

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        setUser: (state, action: PayloadAction<{username: string; given_name:string; email: string; roles: string[]; token: string}>) =>{
            state.user = {
                username: action.payload.username,
                given_name: action.payload.given_name,
                email: action.payload.email,
                roles: action.payload.roles
            };
            state.token = action.payload.token;
            state.isAuthenticated = true;
        },
        logout: (state) => {
            state.user = null;
            state.token = null;
            state.isAuthenticated = false;
        }
    }
});

export const { setUser, logout } = authSlice.actions;
export default authSlice.reducer;