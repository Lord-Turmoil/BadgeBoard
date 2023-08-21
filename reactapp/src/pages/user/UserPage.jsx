import React from 'react';

import { isMobile } from 'react-device-detect';

import UserPagePC from './pc/UserPagePC';
import UserPageMobile from './mobile/UserPageMobile';

export default function UserPage() {
    return (
        <div className="user-main" style={{ position: "relative" }}>
            {isMobile ? <UserPageMobile /> : <UserPagePC />}
        </div>
    );
}