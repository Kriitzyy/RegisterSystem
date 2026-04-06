async function register() {
    const name = document.getElementById("name").value;

    if (!name) {
        alert("Please enter a name");
        return;
    }

    try {
        const response = await fetch("http://localhost:7071/api/registerVisitor", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ name: name })
        });

        if (response.ok) {
            alert("Visitor registered!");
        } else {
            alert("Something went wrong");
        }
    } catch (error) {
        console.error(error);
        alert("Error connecting to server");
    }
}