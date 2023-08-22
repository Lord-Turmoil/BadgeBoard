import { useEffect, useState } from 'react';

import { useLocation, useNavigate } from 'react-router-dom';

import LoginRoundedIcon from '@mui/icons-material/LoginRounded';
import LogoutRoundedIcon from '@mui/icons-material/LogoutRounded';
import HomeRoundedIcon from '@mui/icons-material/HomeRounded';
import FilterFramesRoundedIcon from '@mui/icons-material/FilterFramesRounded';
import AutoModeRoundedIcon from '@mui/icons-material/AutoModeRounded';
import { Avatar, Button, Divider, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';

import api from '~/services/api';
import notifier from '~/services/notifier';
import UserUtil from '~/services/user/UserUtil';
import AvatarUtil from '~/services/user/AvatarUtil';

import './UserNav.css'
import { AppRegistrationRounded, FlareSharp } from '@mui/icons-material';
import PopperMenu from '~/components/layout/PopperMenu';

export default function UserBasicNav({
    user = null
}) {
    const navigate = useNavigate();
    const location = useLocation();

    const [menuAnchor, setMenuAnchor] = useState(null);
    const menuOpen = Boolean(menuAnchor);
    const online = Boolean(user);

    const onClickAvatar = (event) => {
        setMenuAnchor(event.currentTarget);
    };

    const onMenuClose = () => {
        setMenuAnchor(null);
    };

    const onClickBadgeBoard = () => {
        if (!user) {
            notifier.error("Not logged in");
            onMenuClose();
        } else {
            var target = '/user/' + user.account.id;
            if (location.pathname != target) {
                navigate(target);
                window.location.reload(false);
            } else {
                window.location.reload(false);
            }
        }
    }

    const onClickLogout = async () => {
        var dto = await api._post('auth/token/revoke');
        notifier.notifyWithCountDown('c-success', 'See ya later then ~', 5);
        if (dto.meta.status == 0) {
            setTimeout(() => {
                window.location.reload(false);
            }, 5000);
        } else {
            notifier.error(dto.meta.message);
        }
        UserUtil.drop();
        api.dropToken();
        onMenuClose();
    };

    const onClickRefresh = async () => {
        var dto = await api.post('auth/token/refresh');
        notifier.auto(dto.meta);
        onMenuClose();
    };

    const getMenuItems = () => {
        var items = [];
        items.push({
            isDivider: false,
            icon: <HomeRoundedIcon />,
            text: 'Home',
            action: () => { navigate('/') }
        });
        if (online) {
            items.push({ isDivider: false, icon: <FilterFramesRoundedIcon />, text: 'My Board', action: onClickBadgeBoard });
        }
        items.push({ isDivider: true });
        if (online) {
            items.push({ isDivider: false, icon: <AutoModeRoundedIcon />, text: 'Refresh', action: onClickRefresh });
            items.push({ isDivider: false, icon: <LogoutRoundedIcon />, text: 'Logout', action: onClickLogout });
        } else {
            items.push({ isDivider: false, icon: <AppRegistrationRounded />, text: 'Register', action: () => { navigate('/register') } });
            items.push({ isDivider: false, icon: <LoginRoundedIcon />, text: 'Login', action: () => { navigate('/login'); } });
        }
        return items;
    }

    const renderMenuItem = (item, index) => {
        if (item.isDivider) {
            return (<Divider key={index} />);
        } else {
            return (
                <MenuItem onClick={item.action} key={index}>
                    <ListItemIcon>
                        {item.icon}
                    </ListItemIcon>
                    <ListItemText>{item.text}</ListItemText>
                </MenuItem>
            );
        }
    }
    const menuItems = getMenuItems();

    return (
        <div className="UserNav">
            <Button className='UserNav__button' sx={{ borderRadius: '50%' }} onClick={onClickAvatar}>
                <Avatar className='UserNav__avatar'
                    sx={{ width: '55px', height: '55px' }}
                    src={AvatarUtil.getUrlFromUser(user)} />
            </Button>
            <PopperMenu anchorEl={menuAnchor} open={menuOpen} onClose={onMenuClose} transformOrigin="right top">
                {
                    menuItems.map((item, index) => {
                        return renderMenuItem(item, index);
                    })
                }
            </PopperMenu>
        </div >
    );
};