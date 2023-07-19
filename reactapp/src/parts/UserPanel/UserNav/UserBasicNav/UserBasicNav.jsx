import { useEffect, useState } from 'react';

import { Avatar, Button, Divider, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';
import ManageAccountsRoundedIcon from '@mui/icons-material/ManageAccountsRounded';
import LogoutRoundedIcon from '@mui/icons-material/LogoutRounded';

import AvatarUtil from '~/services/user/AvatarUtil';

import '../UserNav.css'
import './UserBasicNav.css'
import api from '~/services/api';
import notifier from '~/services/notifier';
import UserUtil from '~/services/user/UserUtil';


export default function UserBasicNav({
    user = null
}) {

    const [menuAnchor, setMenuAnchor] = useState(null);
    const open = Boolean(menuAnchor && user);

    const onClickAvatar = (event) => {
        setMenuAnchor(event.currentTarget);
    }

    const onMenuClose = () => {
        setMenuAnchor(null);
    }

    const onClickLogout = async () => {
        var dto = await api.post("auth/token/revoke");
        notifier.auto(dto.meta, "See you later then~");
        UserUtil.drop();
        api.dropToken();
        onMenuClose();
        window.location.reload(false);
    }

    const onClickRefresh = async () =>{
        var dto = await api.post("auth/token/refresh");
        notifier.auto(dto.meta);
        onMenuClose();
    }

    return (
        <div className="UserNav UserBasicNav">
            <Button sx={{ borderRadius: "50%" }} onClick={onClickAvatar}>
                <Avatar
                    sx={{ width: "50px", height: "50px" }}
                    src={AvatarUtil.getUrlFromUser(user)}
                />
            </Button>
            <Menu anchorEl={menuAnchor} open={open} onClose={onMenuClose}>
                <MenuItem onClick={onMenuClose}>
                    <ListItemIcon>
                        <ManageAccountsRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Account</ListItemText>
                </MenuItem>
                <Divider />
                <MenuItem onClick={onClickRefresh}>
                    <ListItemIcon>
                        <LogoutRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Refresh</ListItemText>
                </MenuItem>
                <MenuItem onClick={onClickLogout}>
                    <ListItemIcon>
                        <LogoutRoundedIcon />
                    </ListItemIcon>
                    <ListItemText>Logout</ListItemText>
                </MenuItem>
            </Menu>
        </div>
    );
};
