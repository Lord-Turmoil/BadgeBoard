import React from 'react';
import NavBarDev from '../parts/NavBarDev'
import { Helmet } from 'react-helmet';

export default function HomePage() {
    return (
        <div>
            <Helmet>
                <title>Home</title>
            </Helmet>
            <h1>Home Page</h1>
            <NavBarDev></NavBarDev>
        </div>
    );
}
