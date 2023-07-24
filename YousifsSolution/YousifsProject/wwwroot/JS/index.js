
const selectElement = document.getElementById('select-filter');
var sortOn = ["Address", true]
const filter = document.querySelector(".filter");
const maxFloorElement = document.querySelector("#maxFloor");
const minFloorElement = document.querySelector("#minFloor");
var maxFloor = maxFloorElement?.value - 3; // -3 to normalize building height.
var minFloor = minFloorElement?.value - 3;

var allRoofs = document.querySelectorAll(".checkbox-filter");
var myRoofs = "-";
allRoofs?.forEach(o => {
    myRoofs += " " + o.value;
    o.addEventListener("change", function () {
        if (this.checked) {
            myRoofs += " " + this.value;
            getPartialView(sortOn)
        }
        else {
            myRoofs = myRoofs.replace(" " + this.value, "");
            getPartialView(sortOn)
        }
    })
});


maxFloorElement?.addEventListener("change", (event) => {
    maxFloor = event.target.value - 3;
    getPartialView(sortOn)
});
minFloorElement?.addEventListener("change", (event) => {
    minFloor = event.target.value - 3;
    getPartialView(sortOn)
});


selectElement?.addEventListener('change', (event) => {
    let text = event.target.options[event.target.selectedIndex].text;
    console.log(text);
    if (text.slice(-10) === "Descending") {
        sortOn = [event.target.value, false]
        getPartialView(sortOn);
    }
    else {
        sortOn = [event.target.value, true]
        getPartialView(sortOn);
    }
});

getPartialView(sortOn);


function deleteHouse(id) {
    console.log("Deleting");
    fetch(`/delete/${id}`, { method: 'DELETE' })
        .then(async response => console.log(response))
        .then(() => getPartialView(sortOn));
}


async function getPartialView(sort) {

    const superContainer = document.querySelector(".super-container");
    await fetch(`indexpartial/?sort=${sort[0]}&isAscending=${sort[1]}&minFloor=${minFloor}&maxFloor=${maxFloor}&roofs=${myRoofs}`, { method: "GET" }).
        then(result => result.text()).
        then(html => {
            superContainer.innerHTML = html;
        });


    // ------ Makes houses draggable ------------ //
    const draggables = document.querySelectorAll(".draggable");
    const container = document.querySelector(".container");
    draggables.forEach((draggable) => {
        draggable.addEventListener("dragstart", (e) => {
            console.log("test");
            draggable.classList.add("dragging");
        });

        draggable.addEventListener("dragend", () => {
            draggable.classList.remove("dragging");
            saveMove()
            console.log("1")
        });
        container.addEventListener("dragover", (e) => {
            e.preventDefault();
            const afterElement = getDragAfterElement(container, e.clientX);
            const draggable = document.querySelector(".dragging");

            if (afterElement == null) {
                container.appendChild(draggable);
            } else {
                container.insertBefore(draggable, afterElement);
            }
        });
    });


    function getDragAfterElement(container, x) {
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
}

function getMenu(id) {
    const menuElement = document.getElementById(`nav-item-${id}`);
    menuElement.classList.toggle("house-nav-height-10")
}

function saveMove() {
    let ids = [...document.querySelectorAll(".house")].map((o) => o.dataset.id);
    query = "/saveMovings/?";
    for (var i = 0; i < ids.length; i++) {
        query += `idArray=${ids[i]}`
        if (i != ids.length - 1) {
            query += "&"
        }
    }
    console.log(query);
    fetch(query, { method: "POST" })
}

function filterMouseOut() {
    filter.classList.remove("filter-mouse-over")
    filter.classList.add("filter-mouse-out")
}

function filterMouseOver() {
    filter.classList.add("filter-mouse-over")
    filter.classList.remove("filter-mouse-out")
}





