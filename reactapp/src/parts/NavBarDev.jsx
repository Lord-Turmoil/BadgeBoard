import { Link } from '@mui/material';
import React from 'react';


function NavBarDev() {
    return (
        <div id="nav-dev">
            <ul>
                <li>
                    <Link href="/">Home</Link>
                </li>
                <li>
                    <Link href="/register">Register</Link>
                </li>
                <li>
                    <Link href="/login">Login</Link>
                </li>
                <li>
                    <Link href="/user/0">User 0</Link>
                </li>
                <li>
                    <Link href="/404">404</Link>
                </li>
            </ul>
        </div>
    );
}

export default NavBarDev;