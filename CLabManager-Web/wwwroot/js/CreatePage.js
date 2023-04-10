var idCount = 0

$(document).ready(function () {
    if (lab.LabId) {
        console.log("this is room no", lab.RoomNo)
        console.log("this is building no", lab.BuildingNo)
        console.log("this is grid type", lab.GridType)

        if (lab.GridType === 1) {
            drawGridType1()
        } else if (lab.GridType === 2) {
            drawGridType2()
        }
    }
    console.log("this is lab" ,lab.LabId)
    
    $("#computerCreationSave").click(function () {
        let formData = JSON.stringify({
            computerName: $('#computerName').val(),
            description: $('#computerDesc').val()
        })
        console.log(formData)
        $.ajax({
            type: "POST",
            url: "https://localhost:7138/api/Computers",
            headers: {
                'content-type': 'application/json;'
            },
            data: formData,
            success: () => {
                var mydiv = document.querySelector("#unassignedcomps")
                fetch("https://localhost:7138/api/Computers/unassigned")
                    .then(response => response.json())
                    .then(computers => {
                        mydiv.innerHTML = '';
                        computers.map(computer => {
                            console.log(computer)
                            var firstdiv = document.createElement('div')
                            var seconddiv = document.createElement('div')
                            firstdiv.classList.add("m-2", "row")
                            seconddiv.classList.add("col")
                            firstdiv.appendChild(seconddiv)
                            var myimg = document.createElement('img')
                            myimg.id = computer.computerId
                            console.log("image id", myimg.id)
                            myimg.src = "/images/ComputerIcon.png"
                            myimg.alt = "computer"
                            myimg.style.width = "80px"
                            myimg.style.height = "80px"
                            myimg.draggable = true
                            myimg.ondragstart = dragStart_handler
                            seconddiv.appendChild(myimg)
                            console.log("this was ran")
                            mydiv.appendChild(firstdiv)
                        })
                    });
                $('#computerName').val('')
                $('#computerDesc').val('') 
            }
        })
        console.log("ajax request")
    })
})

function dragStart_handler(event) {
    console.log("in dragstart")
    console.log(event)
    event.dataTransfer.dropEffect = "move"
    event.dataTransfer.setData("text/plain", event.target.id)
}


function createSingleAllotment() {
    var box = document.createElement('div');
    box.id = idCount++
    box.style.height = '100px';
    box.style.width = '100px';
    box.style.background = 'grey';
    box.style.border = "10px solid black"
    box.style.margin = "5px";
    box.style.padding = "0px"
    box.classList.add("col-2")
    box.ondragover = function dragOver_handler(event) {
        event.preventDefault()
        event.dataTransfer.dropEffect = "move"
    }
    box.ondrop = function drop_handler(event) {
        event.preventDefault()
        
        if (!box.hasChildNodes()) {
            const data = event.dataTransfer.getData("text/plain");
            var img = document.getElementById(data)
            box.appendChild(img)
            var options = {
                method: "PUT",
                headers: {
                    'content-type':'application/json'
                    },
                body: JSON.stringify({
                    positionOnGrid: box.id,
                    isPositioned: true,
                    labId: lab.LabId
                })
            }
            console.log("this is box id",box.id)
            fetch(`https://localhost:7138/api/Computers/ispositioned/${img.id}`,options)
                .then(() => console.log("fetch executed for drop"))
        } 
        console.log(box)
    }
    return box;
}

function drawGridType1() {
    var container = document.getElementById('gridContainer');  

    function createTeacherRow() {
        console.log("teacher row")
        var teachersRow = document.createElement('div')
        teachersRow.classList.add("row")
        teachersRow.appendChild(createSingleAllotment())
        container.appendChild(teachersRow)
    }

    function createstudentRow() {
        console.log("student row")
        var emptyDiv = document.createElement('div')
        emptyDiv.classList.add("col-2")
        emptyDiv.style.height = '100px'
        emptyDiv.style.width = '100px'
        var studentsRow = document.createElement('div')
        studentsRow.classList.add("row")
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(emptyDiv)
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(createSingleAllotment())
        container.appendChild(studentsRow)
    }

    createTeacherRow()
    createstudentRow()
    createstudentRow()
    createstudentRow()

}

function drawGridType2() {
    var container = document.getElementById('gridContainer');

    function createTeachersRow() {
        var teachersRow = document.createElement('div')
        teachersRow.classList.add("row")
        teachersRow.classList.add("justify-content-center")
        teachersRow.appendChild(createSingleAllotment())
        container.appendChild(teachersRow)

    }
    function createStudentsRow() {

        var studentsRow = document.createElement('div')
        studentsRow.classList.add("row")
        studentsRow.classList.add("d-flex")
        studentsRow.classList.add("justify-content-between")
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(createSingleAllotment())
        container.appendChild(studentsRow)
    }
    createTeachersRow()
    createStudentsRow()
    createStudentsRow()
    createStudentsRow()
    createStudentsRow()

}

