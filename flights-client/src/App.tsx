import React, { useEffect, useState } from 'react';
import { Provider } from 'react-redux';
import { store } from './store/store';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { Container, Typography, Box, Button, CssBaseline } from '@mui/material';
import FlightsTable from './component/flightstable';
import AddFlightForm from './component/addflightform';
import DeleteFlightForm from './component/deleteflightform';
import FlightSearch from './component/flightsearch';
import AuthDialog from './component/authdialog';
import { useAppDispatch, useAppSelector } from './store/hooks';
import { openAddDialog, openDeleteDialog } from './store/uiSlice';

const queryClient = new QueryClient();

function MainApp() {
  const user = useAppSelector(state => state.auth.user);
  const authState = useAppSelector(state => state.auth);
  const dispatch = useAppDispatch();
  const [searchStatus, setSearchStatus] = useState<string | null>(null);
  const [searchDestination, setSearchDestination] = useState<string | null>(null);

  const handleSearch = (status: string | null, destination: string | null) => {
    setSearchStatus(status);
    setSearchDestination(destination);
  };

  const handleClearFilters = () => {
    setSearchStatus(null);
    setSearchDestination(null);
  };

  console.log('Auth state:', authState);
  console.log('User:', user);

  return (
    <Container maxWidth="md" sx={{ bgcolor: "#ffffff", minHeight: "100vh", py: 4 }}>
      <CssBaseline />
      <Typography variant="h3" align="center" color="primary" gutterBottom>
        Flight Board Management
      </Typography>
      {!user ? (
        <Box display="flex" justifyContent="center" alignItems="center" minHeight="60vh">
          <AuthDialog />
        </Box>
      ) : (
        <Box>
          <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
            <Typography variant="h5">Welcome, {user}!</Typography>
            <Button variant="outlined" color="secondary" onClick={() => {
              localStorage.removeItem('user');
              localStorage.removeItem('token');
              window.location.reload();
            }}>
              Logout
            </Button>
          </Box>
          <FlightSearch
            onSearch={handleSearch}
            onClear={handleClearFilters}
          />
          <FlightsTable
            searchStatus={searchStatus}
            searchDestination={searchDestination}
          />
          <Box display="flex" justifyContent="center" gap={2} mt={3}>
            <Button variant="contained" color="primary" onClick={() => dispatch(openAddDialog())}>Add Flight</Button>
            <Button variant="outlined" color="primary" onClick={() => dispatch(openDeleteDialog())}>Delete Flight</Button>
          </Box>
          {/* Dialogs will render here but only show when opened */}
          <AddFlightForm />
          <DeleteFlightForm />
        </Box>
      )}
    </Container>
  );
}

export default function App() {
  return (
    <Provider store={store}>
      <QueryClientProvider client={queryClient}>
        <MainApp />
      </QueryClientProvider>
    </Provider>
  );
}
