////// Variable declarations //////

const selectElement = document.getElementById('select-filter');
const maxFloorElement = document.querySelector("#maxFloor");
const minFloorElement = document.querySelector("#minFloor");
const allRoofs = document.querySelectorAll(".checkbox-filter");
const filter = document.querySelector(".filter");
let sortOn = selectElement?.value;
let selectedOption = selectElement?.options[selectElement.selectedIndex].text
let isAscending = selectedOption?.slice(-10) === "Descending" ? false : true;
let maxFloor = maxFloorElement?.value - 3; // -3 to normalize building height.
let minFloor = minFloorElement?.value - 3;

let includedRoofs = "-";

////// EventListeners //////

allRoofs?.forEach(o => {
    includedRoofs += " " + o.value;
    o.addEventListener("change", function () {
        if (this.checked) {
            includedRoofs += " " + this.value;
            getPartialView()
        }
        else {
            includedRoofs = includedRoofs.replace(" " + this.value, "");
            getPartialView()
        }
    })
});


maxFloorElement?.addEventListener("change", (event) => {
    maxFloor = event.target.value - 3;
    getPartialView()
});
minFloorElement?.addEventListener("change", (event) => {
    minFloor = event.target.value - 3;
    getPartialView()
});


selectElement?.addEventListener('change', (event) => {
    let text = event.target.options[event.target.selectedIndex].text;
    console.log(text);
    if (text.slice(-10) === "Descending") {
        sortOn = event.target.value;
        isAscending = false
        getPartialView();
    }
    else {
        sortOn = event.target.value;
        isAscending = true
        getPartialView();
    }
});

////// Functions //////

function deleteHouse(id) {
    console.log("Deleting");
    fetch(`/delete/${id}`, { method: 'DELETE' }).
        then(async response => {
            console.log(response);
            if (response.ok) {
                return response;
            } else {
                throw new Error('Network response was not ok.');
            }
        }).
        then(() => getPartialView()).
        then(() => {
            //Remove sorting and filter elements if all houses has been deleted
            const containerElement = document.querySelector('.container');
            if (!containerElement) {
                const sortAndFilterContainer = document.getElementById("sort-filter-container");
                sortAndFilterContainer.style.display = "none";
            }
        });
}

async function getPartialView() {

    const superContainer = document.querySelector(".super-container");
    const query = `indexpartial/?sort=${sortOn}&isAscending=${isAscending}&minFloor=${minFloor}&maxFloor=${maxFloor}&roofs=${includedRoofs}`
    console.log(query)
    await fetch(query, { method: "GET" }).
        then(result => result.text()).
        then(html => {
            superContainer.innerHTML = html;
        });

    makeHousesDraggable();
}

function makeHousesDraggable() {
    const draggables = document.querySelectorAll(".draggable");
    const container = document.querySelector(".container");
    draggables.forEach((draggable) => {
        draggable.addEventListener("dragstart", (e) => {
            draggable.classList.add("dragging");
        });

        draggable.addEventListener("dragend", () => {
            draggable.classList.remove("dragging");
            saveMove();
        });
        container.addEventListener("dragover", (e) => {
            e.preventDefault();
            const afterElement = getClosestNonDraggingElement(container, e.clientX);
            const draggable = document.querySelector(".dragging");

            if (afterElement == null) {
                container.appendChild(draggable);
            } else {
                container.insertBefore(draggable, afterElement);
            }
        });
    });
}

function getClosestNonDraggingElement(container, x) {
    const draggableElements = [
        ...container.querySelectorAll(".draggable:not(.dragging)"),
    ];
    return draggableElements.reduce(
        (closest, child) => {
            const box = child.getBoundingClientRect();
            const offset = x - box.left - box.width / 2;
            if (offset < 0 && offset > closest.offset) {
                return { offset: offset, element: child };
            } else {
                return closest;
            }
        },
        {
            offset: Number.NEGATIVE_INFINITY,
        }
    ).element;
}

function getMenu(id) {
    const menuElement = document.getElementById(`nav-item-${id}`);
    menuElement.classList.toggle("house-nav-height-10")
}

async function saveMove() {
    try {
        const query = "api/save-movings"
        const houseIdArray = [...document.querySelectorAll(".house")].map((o) => o.dataset.id);
        // Get the anti-forgery token from a hidden input field
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const headers = new Headers({
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        });

        const fetchConfig = {
            method: 'PATCH',
            headers: headers,
            body: JSON.stringify(houseIdArray)
        };

        const response = await fetch(query, fetchConfig);
        if (!response.ok) {
            throw new Error('Request failed');
        }

        result = await response.json();
        console.log(result.message)
    }
    catch (error) {
        console.log(error);
    }

    // Set the select option to 'Custom order'
    const optionValueToSelect = 'SortingOrder';
    const selectedIndex = Array.from(selectElement.options).findIndex(option => option.value === optionValueToSelect);
    selectElement.selectedIndex = selectedIndex;
}


////// Code Starts Here //////
getPartialView();   
