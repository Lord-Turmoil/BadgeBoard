import React from 'react';
import { useParams } from 'react-router-dom';

function UserPage() {
    const { uid } = useParams();

    return (
        <h1>User Page of {uid}</h1>
    );
}

export default UserPage;
