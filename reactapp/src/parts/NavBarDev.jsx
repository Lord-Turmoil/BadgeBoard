import React from 'react';
import { Link } from 'react-router-dom'

function NavBarDev() {
    return (
        <div id="nav-dev">
            <ul>
                <li><Link to="/">Home</Link></li>
                <li><Link to="/register">Register</Link></li>
                <li><Link to="/signup">Sign Up</Link></li>
                <li><Link to="/login">Login</Link></li>
                <li><Link to="/user/0">User 0</Link></li>
                <li><Link to="/404">404</Link></li>
            </ul>
        </div>
    );
}

export default NavBarDev;