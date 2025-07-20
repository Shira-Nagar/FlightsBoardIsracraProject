// src/types/Flight.ts

export enum FlightStatus {
  Scheduled = 0,
  Boarding = 1,
  Departed = 2,
  Delayed = 3,
  Cancelled = 4
}

export interface Flight {
  id: number;
  flightNumber: string;
  destination: string;
  departure: string;
  departureTime: string;
  arrivalTime: string;
  gate: string;
  status?: string;
}

// אם את שולחת טיסה חדשה בלי ID
export type NewFlight = Omit<Flight, 'id'>;
