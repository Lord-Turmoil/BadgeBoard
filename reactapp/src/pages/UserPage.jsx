import React from 'react';

import { isMobile } from 'react-device-detect';

import UserPagePC from './UserPagePC';
import UserPageMobile from './UserPageMobile';

import '../assets/css/pages/user/user.css'

function UserPage() {
    return (
        <div className='user-main'>
            {isMobile ? <UserPageMobile /> : <UserPagePC />}
        </div>
    );
}

export default UserPage;
