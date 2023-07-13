import React from 'react';
import { isMobile } from 'react-device-detect';
import UserPageMobile from './UserPageMobile';
import UserPagePC from './UserPagePC';

function UserPage() {
    return (
        <div>
            {isMobile ? <UserPageMobile /> : <UserPagePC />}
        </div>
    );
}

export default UserPage;
