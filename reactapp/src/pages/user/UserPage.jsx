import React from 'react';

import { isMobile } from 'react-device-detect';

import UserPagePC from './pc/UserPagePC';
import UserPageMobile from './mobile/UserPageMobile';

function UserPage() {
    return (
        <div className="user-main">
            {isMobile ? <UserPageMobile /> : <UserPagePC />}
        </div>
    );
}

export default UserPage;
