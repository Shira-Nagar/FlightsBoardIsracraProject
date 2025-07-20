import React, { useState } from 'react';
import {
    Box,
    TextField,
    Button,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    SelectChangeEvent,
    Paper,
    Typography
} from '@mui/material';
import { FlightStatus } from '../type/flights';

interface FlightSearchProps {
    onSearch: (status: string | null, destination: string | null) => void;
    onClear: () => void;
}

export default function FlightSearch({ onSearch, onClear }: FlightSearchProps) {
    const [status, setStatus] = useState<string>('');
    const [destination, setDestination] = useState<string>('');

    const handleStatusChange = (event: SelectChangeEvent) => {
        setStatus(event.target.value);
    };

    const handleDestinationChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setDestination(event.target.value);
    };

    const handleSearch = () => {
        onSearch(status || null, destination || null);
    };

    const handleClear = () => {
        setStatus('');
        setDestination('');
        onClear();
    };

    return (
        <Paper elevation={2} sx={{ p: 3, mb: 3 }}>
            <Typography variant="h6" gutterBottom>
                Search & Filter Flights
            </Typography>
            <Box sx={{ display: 'flex', gap: 2, flexWrap: 'wrap', alignItems: 'center' }}>
                <FormControl sx={{ minWidth: 200 }}>
                    <InputLabel>Status</InputLabel>
                    <Select
                        value={status}
                        label="Status"
                        onChange={handleStatusChange}
                    >
                        <MenuItem value="">
                            <em>All Statuses</em>
                        </MenuItem>
                        <MenuItem value="Scheduled">Scheduled</MenuItem>
                        <MenuItem value="Boarding">Boarding</MenuItem>
                        <MenuItem value="Departed">Departed</MenuItem>
                        <MenuItem value="Landed">Landed</MenuItem>
                    </Select>
                </FormControl>

                <TextField
                    label="Destination"
                    value={destination}
                    onChange={handleDestinationChange}
                    placeholder="Enter destination..."
                    sx={{ minWidth: 200 }}
                />

                <Button
                    variant="contained"
                    onClick={handleSearch}
                    sx={{ minWidth: 100 }}
                >
                    Search
                </Button>

                <Button
                    variant="outlined"
                    onClick={handleClear}
                    sx={{ minWidth: 100 }}
                >
                    Clear Filters
                </Button>
            </Box>
        </Paper>
    );
} 