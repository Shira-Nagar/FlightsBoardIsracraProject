const https = require('https');
const http = require('http');

const API_BASE = 'https://localhost:44375/api';
const SIGNALR_URL = 'https://localhost:44375/flighthub';

function checkEndpoint(url, description) {
    return new Promise((resolve) => {
        const client = url.startsWith('https') ? https : http;

        const req = client.get(url, (res) => {
            console.log(`✅ ${description}: ${res.statusCode} ${res.statusMessage}`);
            resolve(true);
        });

        req.on('error', (err) => {
            console.log(`❌ ${description}: ${err.message}`);
            resolve(false);
        });

        req.setTimeout(5000, () => {
            console.log(`⏰ ${description}: Timeout`);
            req.destroy();
            resolve(false);
        });
    });
}

async function checkBackend() {
    console.log('🔍 Checking backend connectivity...\n');

    const apiCheck = await checkEndpoint(`${API_BASE}/flights`, 'API Endpoint');
    const signalrCheck = await checkEndpoint(SIGNALR_URL, 'SignalR Hub');

    console.log('\n📋 Summary:');
    if (apiCheck && signalrCheck) {
        console.log('✅ Backend is running and accessible');
    } else if (apiCheck) {
        console.log('⚠️  API is accessible but SignalR hub is not responding');
    } else {
        console.log('❌ Backend server is not running or not accessible');
        console.log('\n💡 To fix this:');
        console.log('1. Make sure your backend server is running');
        console.log('2. Check if the server is running on https://localhost:44375');
        console.log('3. Verify CORS settings in your backend');
        console.log('4. Check if the SignalR hub is properly configured');
    }
}

checkBackend(); 