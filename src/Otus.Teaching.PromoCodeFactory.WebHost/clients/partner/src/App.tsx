import React, { useEffect, useState } from 'react';
import './App.css';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { Button, TextField, Typography } from '@mui/material';

function App() {
    useEffect(() => {
        setUpSignalRConnection().then((con) => {
            console.log(con.connectionId);
        });
    }, []);

    const [message, setMessage] = useState('...');
    const [code, setCode] = useState('');

    const setUpSignalRConnection = async () => {
        let connection = new HubConnectionBuilder()
            .withUrl("https://localhost:5001/promocodeshub?username=Рыба твоей мечты")
            .withAutomaticReconnect()
            .build();

        connection.on('Message', (message: string) => {
            console.log('Message', message);
            setMessage(message);
        });

        try {
            await connection.start();
        } catch (err) {
            console.log(err);
        }
        return connection;
    };

    const givePromoCodes = () => fetch("https://localhost:5001/api/v1/Promocodes", {
        method: 'POST',
        headers: {
            'content-type': 'application/json;charset=UTF-8',
        },
        body: JSON.stringify({
            serviceInfo: "Очень важная информация",
            partnerName: "Рыба твоей мечты",
            promoCode: code,
            preference: "Театр",
        }),
    });

    return (
        <>
            <Typography variant='h6' fontFamily="fantasy">Message from hub:</Typography>
            <Typography variant='h5' fontFamily="inherit">{message}</Typography>
            <br />
            <Typography variant='h6' fontFamily="fantasy">Request to api:</Typography>
            <TextField label={'promo code'} onChange={(event) => setCode(event.target.value)}></TextField>
            <br />
            <Button variant='contained' onClick={givePromoCodes}>Выдать промокод</Button>
        </>
    );
}

export default App;
