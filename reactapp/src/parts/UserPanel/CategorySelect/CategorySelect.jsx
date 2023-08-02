import { Box, Chip, CircularProgress, FormControl, InputLabel, ListItemIcon, ListItemText, MenuItem, Select, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import LockRoundedIcon from '@mui/icons-material/LockRounded';
import PublicRoundedIcon from '@mui/icons-material/PublicRounded';
import './CategorySelect.css';
import { ExpandMoreRounded, Height } from "@mui/icons-material";
import PopperMenu from "~/components/layout/PopperMenu";

export default function CategorySelect({
    categories,
    currentCategory,
    setCategoryIndex
}) {
    const isReady = Boolean(categories && currentCategory);

    // select panel
    const [panelAnchor, setPanelAnchor] = useState(null);
    const panelOpen = Boolean(panelAnchor);
    const onSelectPanelClose = () => {
        setPanelAnchor(null);
    };
    const onClickSelect = (event) => {
        setPanelAnchor(event.currentTarget);
    };
    const handleCategoryChange = (id) => {
        (() => {
            setCategoryIndex && setCategoryIndex(id);
            onSelectPanelClose();
        })();
    };

    // renderer
    const renderCategory = (category) => {
        if (category == null) {
            return null;
        }
        return (
            <div className="CategorySelect__select">
                {category.option.isPublic ? <PublicRoundedIcon className="CategorySelect__icon" /> : <LockRoundedIcon className="CategorySelect__icon"/>}
                <div className="CategorySelect__name">{category.name}</div>
                <ExpandMoreRounded className="CategorySelect__expand" fontSize="large" sx={{
                    transition: 'transform 0.3s',
                    transform: panelOpen ? 'rotate(0deg)' : 'rotate(180deg)'
                }} />
            </div>
        );
    }
    return (
        <div className="CategorySelect">
            <div className="CategorySelect__chip" onClick={onClickSelect}>
                {renderCategory(currentCategory)}
            </div>
            <PopperMenu anchorEl={panelAnchor} open={panelOpen} onClose={onSelectPanelClose}>
                {categories &&
                    categories.map((category, index) =>
                    (<MenuItem value={index} key={category.id} onClick={() => handleCategoryChange(index)}>
                        <ListItemIcon>
                            {category.option.isPublic ? <PublicRoundedIcon /> : <LockRoundedIcon />}
                        </ListItemIcon>
                        <ListItemText>{category.name}</ListItemText>
                    </MenuItem>))
                }
            </PopperMenu>
            {isReady ?
                null
                :
                <div className="CategorySelect__loading">
                    <p className="CategorySelect__prompt">Loading categories...</p>
                    <CircularProgress
                        size={32}
                        sx={{
                            color: "primary",
                            position: 'absolute',
                            top: '50%',
                            left: '50%',
                            marginTop: '-16px',
                            marginLeft: '-16px',
                        }}
                    />
                </div>
            }
        </div>
    );
}