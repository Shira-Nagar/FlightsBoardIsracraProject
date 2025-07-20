import React, { useState } from 'react';
import {
  TextField,
  Button,
  Box,
  Typography,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { createFlight } from '../api/flightsapi';
import { NewFlight } from '../type/flights';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { closeAddDialog } from '../store/uiSlice';
import { useQueryClient } from '@tanstack/react-query';

const defaultFlight: NewFlight = {
  flightNumber: '',
  destination: '',
  departure: '',
  departureTime: '',
  arrivalTime: '',
  gate: '',
};

const NewFlightForm = () => {
  const dispatch = useAppDispatch();
  const queryClient = useQueryClient();
  const isOpen = useAppSelector(state => state.ui.addDialogOpen);
  const [formData, setFormData] = useState<NewFlight>(defaultFlight);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleClose = () => {
    dispatch(closeAddDialog());
    setFormData(defaultFlight);
    setError(null);
    setSuccess(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);
    setIsSubmitting(true);

    try {
      await createFlight(formData);
      setSuccess(true);
      setFormData(defaultFlight);
      // Refresh the flights list
      queryClient.invalidateQueries({ queryKey: ['flights'] });
      // Close dialog after a short delay
      setTimeout(() => {
        handleClose();
      }, 1500);
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('Unknown error occurred');
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Dialog open={isOpen} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Typography variant="h6">הוסף טיסה חדשה</Typography>
          <IconButton onClick={handleClose} size="small">
            <CloseIcon />
          </IconButton>
        </Box>
      </DialogTitle>
      <DialogContent>
        <Box component="form" onSubmit={handleSubmit} sx={{ pt: 1 }}>
          <TextField
            label="מספר טיסה"
            name="flightNumber"
            value={formData.flightNumber}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            label="נקודת יציאה"
            name="departure"
            value={formData.departure}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            label="יעד"
            name="destination"
            value={formData.destination}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
          <TextField
            type="datetime-local"
            label="זמן יציאה"
            name="departureTime"
            value={formData.departureTime}
            onChange={handleChange}
            fullWidth
            margin="normal"
            InputLabelProps={{ shrink: true }}
            required
          />
          <TextField
            type="datetime-local"
            label="זמן הגעה"
            name="arrivalTime"
            value={formData.arrivalTime}
            onChange={handleChange}
            fullWidth
            margin="normal"
            InputLabelProps={{ shrink: true }}
            required
          />
          <TextField
            label="שער"
            name="gate"
            value={formData.gate}
            onChange={handleChange}
            fullWidth
            margin="normal"
            required
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} color="inherit">
          ביטול
        </Button>
        <Button
          type="submit"
          variant="contained"
          color="primary"
          onClick={handleSubmit}
          disabled={isSubmitting}
        >
          {isSubmitting ? 'מוסיף...' : 'הוסף טיסה'}
        </Button>
      </DialogActions>

      {success && (
        <Box sx={{ p: 2, bgcolor: 'success.light', color: 'success.contrastText' }}>
          <Typography>הטיסה נוספה בהצלחה!</Typography>
        </Box>
      )}
      {error && (
        <Box sx={{ p: 2, bgcolor: 'error.light', color: 'error.contrastText' }}>
          <Typography>{error}</Typography>
        </Box>
      )}
    </Dialog>
  );
};

export default NewFlightForm;
