import React from 'react';

import { Helmet } from 'react-helmet';

import NavBarDev from '../parts/NavBarDev'

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
