
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getFlights, searchFlights, deleteFlight } from '../api/flightsapi';
import {
    Paper,
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    CircularProgress,
    Typography,
    Chip,
    Fade,
    IconButton,
    Tooltip
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import type { Flight } from '../type/flights';
import type { AxiosError } from 'axios'; // אם את משתמשת ב־axios

// Returns color for flight status
const getStatusColor = (status?: string) => {
    switch (status?.toLowerCase()) {
        case 'scheduled':
            return 'primary';
        case 'boarding':
            return 'warning';
        case 'departed':
            return 'info';
        case 'landed':
            return 'success';
        default:
            return 'default';
    }
};

interface FlightsTableProps {
    searchStatus?: string | null;
    searchDestination?: string | null;
}

// טיפוס של הקונטקסט בין onMutate ל-onError
type ContextType = {
    previousFlights: Flight[] | undefined;
};

export default function FlightsTable({ searchStatus, searchDestination }: FlightsTableProps) {
    const queryClient = useQueryClient();

    const { data: flights, isLoading, isError } = useQuery<Flight[]>({
        queryKey: ['flights', searchStatus, searchDestination],
        queryFn: () => {
            if (searchStatus || searchDestination) {
                return searchFlights(searchStatus || undefined, searchDestination || undefined);
            }
            return getFlights();
        },
    });

    const deleteMutation = useMutation<void, AxiosError, string, ContextType>({
        mutationFn: deleteFlight,

        onMutate: async (flightId) => {
            await queryClient.cancelQueries({ queryKey: ['flights'] });

            const previousFlights = queryClient.getQueryData<Flight[]>(['flights']);

            queryClient.setQueryData(['flights'], (old: Flight[] | undefined) => {
                return old?.filter((flight) => flight.id.toString() !== flightId);
            });

            return { previousFlights };
        },

        onError: (err, flightId, context) => {
            queryClient.setQueryData(['flights'], context?.previousFlights);
        },

        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ['flights'] });
        },
    });

    const handleDelete = (flightId: string) => {
        if (window.confirm('האם אתה בטוח שברצונך למחוק טיסה זו?')) {
            deleteMutation.mutate(flightId);
        }
    };

    if (isLoading) return <CircularProgress sx={{ display: 'block', mx: 'auto', mt: 4 }} />;
    if (isError) return <Typography color="error">שגיאה בטעינת טיסות</Typography>;
    if (!flights || flights.length === 0) return <Typography sx={{ textAlign: 'center', mt: 4 }}>אין טיסות זמינות</Typography>;

    const validFlights = flights.filter((flight, index, array) => {
        return flight && flight.id && array.findIndex(f => f.id === flight.id) === index;
    });

    return (
        <Paper elevation={3} sx={{ mt: 4 }}>
            <Table>
                <TableHead sx={{ bgcolor: '#e3f2fd' }}>
                    <TableRow>
                        <TableCell>Flight #</TableCell>
                        <TableCell>Destination</TableCell>
                        <TableCell>Departure Time</TableCell>
                        <TableCell>Gate</TableCell>
                        <TableCell>Status</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {validFlights.map((f, index) => (
                        <Fade in={true} timeout={300 + index * 100} key={`${f.id}-${index}`}>
                            <TableRow>
                                <TableCell>{f.flightNumber}</TableCell>
                                <TableCell>{f.destination}</TableCell>
                                <TableCell>{new Date(f.departureTime).toLocaleString()}</TableCell>
                                <TableCell>{f.gate}</TableCell>
                                <TableCell>
                                    <Chip
                                        label={f.status || 'Unknown'}
                                        color={getStatusColor(f.status)}
                                        size="small"
                                    />
                                </TableCell>
                                <TableCell>
                                    <Tooltip title="Delete Flight">
                                        <IconButton
                                            onClick={() => handleDelete(f.id.toString())}
                                            color="error"
                                            size="small"
                                            disabled={deleteMutation.isPending}
                                        >
                                            <DeleteIcon />
                                        </IconButton>
                                    </Tooltip>
                                </TableCell>
                            </TableRow>
                        </Fade>
                    ))}
                </TableBody>
            </Table>
        </Paper>
    );
}

