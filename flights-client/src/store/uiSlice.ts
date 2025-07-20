import { createSlice } from '@reduxjs/toolkit';

interface UIState {
    addDialogOpen: boolean;
    deleteDialogOpen: boolean;
}

const initialState: UIState = {
    addDialogOpen: false,
    deleteDialogOpen: false,
};

// UI slice for dialog state
const uiSlice = createSlice({
    name: 'ui',
    initialState,
    reducers: {
        openAddDialog(state) {
            state.addDialogOpen = true;
        },
        closeAddDialog(state) {
            state.addDialogOpen = false;
        },
        openDeleteDialog(state) {
            state.deleteDialogOpen = true;
        },
        closeDeleteDialog(state) {
            state.deleteDialogOpen = false;
        },
    },
});

export const {
    openAddDialog,
    closeAddDialog,
    openDeleteDialog,
    closeDeleteDialog,
} = uiSlice.actions;

export default uiSlice.reducer; 