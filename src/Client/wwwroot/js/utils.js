window.saveToLocalStorage = (key, value) => {
    localStorage.setItem(key, value);
}

window.getFromLocalStorage = (key) => {
    const value = localStorage.getItem(key);
    return value === null ? "" : value;
}

window.removeFromLocalStorage = (key) => {
    localStorage.removeItem(key);
}