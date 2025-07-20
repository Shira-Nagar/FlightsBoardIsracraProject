import axios from 'axios';
import type { NewFlight } from '../type/flights';


const API_BASE = 'https://localhost:44375/api';

export const api = axios.create({
    baseURL: API_BASE,
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Set the authentication token for API requests
export function setAuthToken(token: string | null) {
    if (token) {
        api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    } else {
        delete api.defaults.headers.common['Authorization'];
    }
}

// Load token from localStorage on app start
const savedToken = localStorage.getItem('token');
if (savedToken) {
    setAuthToken(savedToken);
}

// Login user and return token
export async function login(Username: string, Password: string) {
    console.log('Making login request to:', `${API_BASE}/User/Login`);
    console.log('Request data:', { Username, Password });
    const res = await api.post('User/Login', { Username, Password });
    console.log('Login response status:', res.status);
    console.log('Login response data:', res.data);
    return res.data;
}

export async function register(username: string, password: string) {
    console.log('Making register request to:', `${API_BASE}/User/SignUp`);
    console.log('Request data:', { username, password });
    const res = await api.post('User/SignUp', { username, password });
    console.log('Register response status:', res.status);
    console.log('Register response data:', res.data);
    return res.data;
}

export async function getFlights() {
    try {
        console.log('Attempting to fetch flights from:', `${API_BASE}/flights`);
        const res = await api.get('flights');
        console.log('Flights response:', res.data);
        // Ensure we return an array and filter out any invalid entries
        const flights = Array.isArray(res.data) ? res.data : [];
        return flights.filter(flight => flight && typeof flight === 'object' && flight.id);
    } catch (error: any) {
        console.error('Error fetching flights:', error);
        if (error.response) {
            console.error('Response status:', error.response.status);
            console.error('Response data:', error.response.data);
        }
        throw error;
    }
}

export async function searchFlights(status?: string | null, destination?: string | null) {
    try {
        const params = new URLSearchParams();
        if (status) params.append('status', status);
        if (destination) params.append('destination', destination);

        const res = await api.get(`flights/search?${params.toString()}`);
        // Ensure we return an array and filter out any invalid entries
        const flights = Array.isArray(res.data) ? res.data : [];
        return flights.filter(flight => flight && typeof flight === 'object' && flight.id);
    } catch (error) {
        console.error('Error searching flights:', error);
        throw error;
    }
}

export const createFlight = async (flight: NewFlight): Promise<void> => {
    await api.post('flights', flight);
};

export async function deleteFlight(id: string) {
    const res = await api.delete(`flights/${id}`);
    return res.data;
}
