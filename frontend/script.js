async function register() {
    const name = document.getElementById("name").value;

    if (!name) {
        alert("Please enter a name");
        return;
    }

    try {
        const response = await fetch("system-function-bggcfsguera8f8de.canadacentral-01.azurewebsites.net/api/registerVisitor", {
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