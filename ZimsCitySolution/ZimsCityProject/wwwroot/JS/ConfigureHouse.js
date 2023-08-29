const inputRange = document.getElementById("house-width");

console.log(inputRange)
const houseWidthLabel = document.getElementById("house-width-label");

inputRange.addEventListener("input", (event) => {
    houseWidthLabel.innerHTML = `House Width: ${event.target.value}m`
} )