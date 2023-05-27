import React, { useEffect, useState } from 'react';
import './App.css';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { Button, TextField, Typography } from '@mui/material';

function App() {
  const defaultEmail = 'ivan_sergeev@mail.ru';
  const [message, setMessage] = useState('...');
  const [email, setEmail] = useState(defaultEmail);
  const [promocodes, setPromocodes] = useState<{ code: string, partner: string }[]>([]);

  const setUpSignalRConnection = async (email: string) => {
    let connection = new HubConnectionBuilder()
      .withUrl(`https://localhost:5001/promocodeshub?username=${email}`)
      .withAutomaticReconnect()
      .build();

    connection.on('Message', (message: string) => {
      setMessage(message);
    });
    connection.on('PromoCodes', (promoCodes: [{ code: string, partner: string }]) => {
      setPromocodes(promoCodes);
    });

    try {
      await connection.start();
    } catch (err) {
      console.log(err);
    }
  };

  return (
    <>
      <Typography variant='h6' fontFamily="fantasy">Message from hub:</Typography>
      <Typography variant='h5' fontFamily="inherit">{message}</Typography>
      {/* <Typography variant='h6' fontFamily="fantasy">Your promo codes:</Typography>
      <Typography variant='h5' fontFamily="inherit">{JSON.stringify(promocodes)}</Typography> */}
      <br />
      <Typography variant='h6' fontFamily="fantasy">Request to api:</Typography>
      <Typography variant='h6' fontFamily="fantasy">Enter the email to show your promocodes</Typography>
      <TextField
        defaultValue={defaultEmail}
        label={'email'}
        onChange={(event) => setEmail(event.target.value)}>
      </TextField>
      <br />
      <Button variant='contained' onClick={() => setUpSignalRConnection(email)}>Подключиться</Button>
    </>
  );
}

export default App;
