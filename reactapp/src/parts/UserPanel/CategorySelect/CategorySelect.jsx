import { Box, CircularProgress, FormControl, InputLabel, MenuItem, Select, Typography } from "@mui/material";
import { useEffect, useState } from "react";

import './CategorySelect.css';

export default function CategorySelect({
    categories,
    currentCategory,
    setCategory
}) {
    const handleChange = (event) => {
        setCategory(event.target.value);
    };

    const isReady = () => {
        return categories != null && currentCategory != null;
    }

    return (
        <div className="CategorySelect">
            {isReady() ?
                <FormControl fullWidth>
                    <InputLabel>Category</InputLabel>
                    <Select
                        value={currentCategory}
                        label="Category"
                        onChange={handleChange}
                    >
                        {categories &&
                            categories.map((category, index) =>
                                (<MenuItem value={index} key={category.id}>{category.name}</MenuItem>)
                            )
                        }
                    </Select>
                </FormControl>
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