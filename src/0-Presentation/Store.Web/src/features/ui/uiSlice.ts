import { createSlice } from '@reduxjs/toolkit';

interface UIState {
    sidebarCollapsed: boolean;
}

const initialState: UIState = {
    sidebarCollapsed: localStorage.getItem('sidebarCollapsed') === 'true'
};

const uiSlice = createSlice({
    name: 'ui',
    initialState,
    reducers: {
        toggleSidebar: (state) => {
            state.sidebarCollapsed = !state.sidebarCollapsed;
            localStorage.setItem('sidebarCollapsed', state.sidebarCollapsed.toString());
        }
    }
});

export const { toggleSidebar } = uiSlice.actions;
export default uiSlice.reducer;