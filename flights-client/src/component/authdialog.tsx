import React, { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, TextField, Button, Box, Typography } from '@mui/material';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { loginStart, loginSuccess, loginFailure, registerStart, registerSuccess, registerFailure } from '../store/authSlice';
import { login, register, setAuthToken } from '../api/flightsapi';
import { AxiosError } from 'axios';
// Authentication dialog for login/register
export default function AuthDialog() {
    const dispatch = useAppDispatch();
    const { loading, error } = useAppSelector(state => state.auth);
    const [registerMode, setRegisterMode] = useState(false);
    const [form, setForm] = useState({ username: '', password: '' });
    const [localError, setLocalError] = useState('');

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async () => {
        if (!form.username || !form.password) {
            setLocalError('Please enter username and password');
            return;
        }
        setLocalError('');
        if (registerMode) {
            dispatch(registerStart());
            try {
                console.log('Attempting to register:', form.username);
                const data = await register(form.username, form.password);
                console.log('Register response:', data);
                if (data.token) {
                    setAuthToken(data.token);
                    dispatch(registerSuccess({ user: form.username, token: data.token }));
                } else {
                    dispatch(registerFailure('No token received from server'));
                }
            } catch (err: unknown) {
                console.error('Register error:', err);
                let errorMessage = 'Registration error';
                if (err instanceof AxiosError) {
                    errorMessage = err.response?.data?.message || err.response?.data || errorMessage;
                }
                dispatch(registerFailure(errorMessage));
            }
        } else {
            dispatch(loginStart());
            try {
                console.log('Attempting to login:', form.username);
                const data = await login(form.username, form.password);
                console.log('Login response:', data);
                console.log('Response type:', typeof data);
                console.log('Response keys:', Object.keys(data || {}));
                if (data && data.token) {
                    console.log('Token received:', data.token);
                    setAuthToken(data.token);
                    console.log('About to dispatch loginSuccess');
                    dispatch(loginSuccess({ user: form.username, token: data.token }));
                    console.log('loginSuccess dispatched');
                } else {
                    console.log('No token in response');
                    dispatch(loginFailure('No token received from server'));
                }
            } catch (err: unknown) {
                console.error('Login error:', err);
                let errorMessage = 'Login error';
                if (err instanceof AxiosError) {
                    errorMessage = err.response?.data?.message || err.response?.data || errorMessage;
                }
                dispatch(loginFailure(errorMessage));
            }
        }
    };

    return (
        <Dialog open={true} fullWidth maxWidth="sm">
            <DialogTitle align="center" color="primary">
                {registerMode ? 'Register' : 'Login'}
            </DialogTitle>
            <DialogContent>
                <Box display="flex" flexDirection="column" gap={2} mt={1}>
                    <TextField label="Username" name="username" value={form.username} onChange={handleChange} fullWidth />
                    <TextField label="Password" name="password" type="password" value={form.password} onChange={handleChange} fullWidth />
                    <Button variant="contained" color="primary" onClick={handleSubmit} disabled={loading}>
                        {loading ? 'Loading...' : (registerMode ? 'Register' : 'Login')}
                    </Button>
                    {(localError || error) && <Typography color="error" align="center">{localError || error}</Typography>}
                    <Typography align="center" variant="body2">
                        {registerMode ? (
                            <>
                                Already have an account?{' '}
                                <Button color="primary" onClick={() => setRegisterMode(false)}>Login</Button>
                            </>
                        ) : (
                            <>
                                Don't have an account?{' '}
                                <Button color="primary" onClick={() => setRegisterMode(true)}>Register</Button>
                            </>
                        )}
                    </Typography>
                    <Typography align="center" variant="caption" color="textSecondary">
                        Default user: admin / admin123
                    </Typography>
                </Box>
            </DialogContent>
        </Dialog>
    );
} 