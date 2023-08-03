import { useEffect, useState } from 'react';

import { useNavigate } from 'react-router-dom';

import LoginRoundedIcon from '@mui/icons-material/LoginRounded';
import LogoutRoundedIcon from '@mui/icons-material/LogoutRounded';
import HomeRoundedIcon from '@mui/icons-material/HomeRounded';
import { Avatar, Button, Divider, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';

import api from '~/services/api';
import notifier from '~/services/notifier';
import UserUtil from '~/services/user/UserUtil';
import AvatarUtil from '~/services/user/AvatarUtil';

import './UserNav.css'
import { AppRegistrationRounded } from '@mui/icons-material';
import PopperMenu from '~/components/layout/PopperMenu';

export default function UserBasicNav({
    user = null
}) {
    const navigate = useNavigate();

    const [menuAnchor, setMenuAnchor] = useState(null);
    const onlineOpen = Boolean(menuAnchor && user);
    const offlineOpen = Boolean(menuAnchor && !user);

    const onClickAvatar = (event) => {
        setMenuAnchor(event.currentTarget);
    };

    const onMenuClose = () => {
        setMenuAnchor(null);
    };

    const onClickLogout = async () => {
        var dto = await api._post('auth/token/revoke');
        notifier.auto(dto.meta, 'See you later then~');
        if (dto.meta.status == 0) {
            notifier.notifyWithCountDown('c-success', 'See ya later then ~', 5);
        } else {
            notifier.error(dto.meta.message);
        }
        UserUtil.drop();
        api.dropToken();
        onMenuClose();
        setTimeout(() => {
            window.location.reload(false);
        }, 5000);
    };

    const onClickRefresh = async () => {
        var dto = await api.post('auth/token/refresh');
        notifier.auto(dto.meta);
        onMenuClose();
    };

    return (
        <div className="UserNav">
            <Button sx={{ borderRadius: '50%' }} onClick={onClickAvatar}>
                <Avatar
                    sx={{ width: '55px', height: '55px' }}
                    src={AvatarUtil.getUrlFromUser(user)} />
            </Button>
            <PopperMenu anchorEl={menuAnchor} open={onlineOpen} onClose={onMenuClose} transformOrigin="right top">
                <MenuItem onClick={() => { navigate('/') }}>
                    <ListItemIcon>
                        <HomeRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Home</ListItemText>
                </MenuItem>
                <Divider />
                <MenuItem onClick={onClickLogout}>
                    <ListItemIcon>
                        <LogoutRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Logout</ListItemText>
                </MenuItem>
            </PopperMenu>
            <PopperMenu anchorEl={menuAnchor} open={offlineOpen} onClose={onMenuClose} transformOrigin="right top">
                <MenuItem onClick={() => { navigate('/') }}>
                    <ListItemIcon>
                        <HomeRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Home</ListItemText>
                </MenuItem>
                <Divider />
                <MenuItem onClick={() => { navigate('/register'); }}>
                    <ListItemIcon>
                        <AppRegistrationRounded />
                    </ListItemIcon>
                    <ListItemText>Register</ListItemText>
                </MenuItem>
                <MenuItem onClick={() => { navigate('/login'); }}>
                    <ListItemIcon>
                        <LoginRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Login</ListItemText>
                </MenuItem>
            </PopperMenu>
        </div>
    );
};