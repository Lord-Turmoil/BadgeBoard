import { Box, Chip, CircularProgress, FormControl, InputLabel, ListItemIcon, ListItemText, MenuItem, Select, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import LockRoundedIcon from '@mui/icons-material/LockRounded';
import PublicRoundedIcon from '@mui/icons-material/PublicRounded';
import './CategorySelect.css';
import { Height } from "@mui/icons-material";
import PopperMenu from "~/components/layout/PopperMenu";

export default function CategorySelect({
    categories,
    currentCategory,
    setCategoryIndex
}) {
    const handleChange = (id) => {
        setCategoryIndex && setCategoryIndex(id);
    };

    const isReady = Boolean(categories && currentCategory);

    // select panel
    const [panelAnchor, setPanelAnchor] = useState(null);
    const panelOpen = Boolean(panelAnchor);
    const onSelectPanelClose = () => {
        setPanelAnchor(null);
    };
    const onClickChip = (event) => {
        setPanelAnchor(event.currentTarget);
    };
    return (
        <div className="CategorySelect">
            <Chip className="CategorySelect__chip"
                clickable
                label={currentCategory && currentCategory.name}
                onClick={onClickChip} />
            <PopperMenu anchorEl={panelAnchor} open={panelOpen} onClose={onSelectPanelClose}>
                {categories &&
                    categories.map((category, index) =>
                    (<MenuItem value={index} key={category.id} onClick={() => handleChange(index)}>
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