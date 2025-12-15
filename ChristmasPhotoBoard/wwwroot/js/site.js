function ImageUpload() {
  if (!fileInput.files || fileInput.files.length === 0) {
    alert('Please select an image.');
    return;
  }

  const formData = new FormData();
  formData.append('file', fileInput.files[0]);

  //ShowOverlay('Wait...');

  fetch('/Home/ImageUpload', {
    method: 'POST',
    body: formData
  })
    .then(response => {
      console.log(response);
      if (!response.ok) throw new Error('Network response was not ok');
      return response.text();
    })
    .then(summary => {
      fileInput.value = '';
      //HideOverlay();
    })
    .catch(error => {
      console.log(error);
    });
}
