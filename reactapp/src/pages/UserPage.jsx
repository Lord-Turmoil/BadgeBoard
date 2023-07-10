import React from 'react';
import { useParams } from 'react-router-dom';
import NavBarDev from '../parts/NavBarDev'

function UserPage() {
    const { uid } = useParams();

    return (
        <div>
            <h1>User Page of {uid}</h1>
            <NavBarDev></NavBarDev>
        </div>
    );
}

export default UserPage;
