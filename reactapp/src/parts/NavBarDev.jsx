import { Link } from '@mui/material';
import React from 'react';


function NavBarDev() {
    const getUserId = () => {
        return window.localStorage.getItem("uid");
    }
    const getUserLink = () => {
        const id = getUserId();
        return "/user/" + id;
    }
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
                    <Link href={getUserLink()}>User {getUserId()}</Link>
                </li>
                <li>
                    <Link href="/404">404</Link>
                </li>
            </ul>
        </div>
    );
}

export default NavBarDev;