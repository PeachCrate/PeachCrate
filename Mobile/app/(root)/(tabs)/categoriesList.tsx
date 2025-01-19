import React, {useEffect, useState} from 'react';
import {FlatList} from 'react-native';
import {TextInput, Button, Card, Title, Paragraph, useTheme, Text} from 'react-native-paper';
import {useDeleteCategoryMutation, useGetCategoriesQuery} from "@/behavior/category/categoryApi";
import {SafeAreaView} from "react-native-safe-area-context";
import {useNavigation, useRouter} from "expo-router";
import {SignIn} from "@clerk/clerk-react";
import {SignedIn, SignedOut} from "@clerk/clerk-expo";

const CategoryListScreen = () => {
  const theme = useTheme();
  const [search, setSearch] = useState('');
  const [orderBy, setOrderBy] = useState('');
  const [pageNum, setPageNum] = useState(10);
  const [pageStart, setPageStart] = useState(0);
  const router = useRouter();

  const {data, isLoading, refetch, error} = useGetCategoriesQuery({
    filterValue: search,
    orderBy,
    pageNum,
    pageStart,
  }, {refetchOnMountOrArgChange: true});
  if (error)
    console.log('ERROR', error);
  const [deleteCategory] = useDeleteCategoryMutation();

  useEffect(() => {
    refetch()
  }, []);

  const handleDelete = async (id: number) => {
    await deleteCategory(id);
    refetch();
  };

  const handleEdit = (id: number) => {
    router.navigate(`/(root)/category/${id}`);
    refetch();
  };

  const handleCreate = () => {
    router.navigate(`/(root)/category/0`);
    refetch();
  };
  
  

  return (
    <SafeAreaView className="flex-1 p-4 ">
      <SignedIn>
        
      <Button onPress={refetch}>Reload</Button>
      <Button mode='contained' onPress={handleCreate}>Create category</Button>
      <TextInput
        label="Search"
        value={search}
        onChangeText={setSearch}
        className="mb-4"
      />
      <TextInput
        label="Order By"
        value={orderBy}
        onChangeText={setOrderBy}
        className="mb-4"
      />
      <Button mode="contained" onPress={() => refetch()} className="mb-4">
        Apply Filters
      </Button>
      {isLoading ? (
        <Paragraph>Loading...</Paragraph>
      ) : (
        <FlatList
          data={data!}
          keyExtractor={(item) => item.categoryId.toString()}
          renderItem={({item}) => (
            <Card className="mb-4">
              <Card.Content>
                <Title>{item.title}</Title>
                <Paragraph>{item.description}</Paragraph>
              </Card.Content>
              <Card.Actions>
                <Button onPress={() => handleEdit(item.categoryId)}>Edit</Button>
                <Button onPress={() => handleDelete(item.categoryId)}>Delete</Button>
              </Card.Actions>
            </Card>
          )}
        />
      )}
      </SignedIn>
      <SignedOut>
        <Text>You need to be logged in for accessing this page</Text>
      </SignedOut>
    </SafeAreaView>
  );
};

export default CategoryListScreen;