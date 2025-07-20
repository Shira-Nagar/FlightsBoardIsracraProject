import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, TextField, Button, Box } from '@mui/material';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { deleteFlight } from '../api/flightsapi';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { closeDeleteDialog } from '../store/uiSlice';

// Form dialog for deleting a flight
export default function DeleteFlightForm() {
    const open = useAppSelector(state => state.ui.deleteDialogOpen);
    const dispatch = useAppDispatch();
    const queryClient = useQueryClient();
    const [flightId, setFlightId] = useState('');
    const [error, setError] = useState('');

    const mutation = useMutation({
        mutationFn: deleteFlight,
        onMutate: async (flightId) => {
            // Cancel any outgoing refetches
            await queryClient.cancelQueries({ queryKey: ['flights'] });

            // Snapshot the previous value
            const previousFlights = queryClient.getQueryData(['flights']);

            // Optimistically update to the new value
            queryClient.setQueryData(['flights'], (old: any) => {
                return old?.filter((flight: any) => flight.id.toString() !== flightId);
            });

            // Return a context object with the snapshotted value
            return { previousFlights };
        },
        onError: (err: any, flightId, context) => {
            // If the mutation fails, use the context returned from onMutate to roll back
            queryClient.setQueryData(['flights'], context?.previousFlights);
            setError(err?.response?.data || 'Error deleting flight');
        },
        onSettled: () => {
            // Always refetch after error or success
            queryClient.invalidateQueries({ queryKey: ['flights'] });
        },
        onSuccess: () => {
            dispatch(closeDeleteDialog());
            setFlightId('');
            setError('');
        },
    });

    const handleSubmit = () => {
        if (!flightId) {
            setError('Please enter flight ID');
            return;
        }
        mutation.mutate(flightId);
    };

    return (
        <Dialog open={open} onClose={() => dispatch(closeDeleteDialog())} fullWidth>
            <DialogTitle>Delete Flight</DialogTitle>
            <DialogContent>
                <Box display="flex" flexDirection="column" gap={2} mt={1}>
                    <TextField label="Flight ID" value={flightId} onChange={e => setFlightId(e.target.value)} fullWidth />
                    {error && <Box color="error.main">{error}</Box>}
                </Box>
            </DialogContent>
            <DialogActions>
                <Button onClick={() => dispatch(closeDeleteDialog())}>Cancel</Button>
                <Button variant="contained" color="primary" onClick={handleSubmit} disabled={mutation.status === 'pending'}>Delete</Button>
            </DialogActions>
        </Dialog>
    );
} 